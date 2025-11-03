# EDC - Electronics Data Capture Device Communication Library
Client library to connect to pos terminal (ISO-8583) devices.

## Usage

### Reference
Add references to the following namespaces
```c#
using Edc.Client;
using Edc.Core.Common;
using Edc.Core.Messages;
```

### Create Client
Create client instance by entering terminal device's IP and port.
```c#
ITransport transport = new TcpTransport(ip, port);
client = new EdcClient(transport);
```

### Connection Test Message
To test the coneection between pos app and the terminal, ConnectionRequestMessage can be sent.
You can send optional parameters (date/time, POS ID) or you can use default parameter.
Please note that this is just to verify that connection is ok for the very first time.
You do not need to send this message before every other transaction messages.
Client will automatically handle connection under the hood.
```c#
// using default parameter values
var requestMessage = new ConnectionRequestMessage();

// accept returning message, you need to cast to the correct response message
var responseMessage = (ConnectionResponseMessage) await client.SendRequestAsync(requestMessage);

// then check for response code
if(responseMessage.ResponseCode == ResponseCodes.APPROVED)
{
    Console.WriteLine("Connection OK!");
}
```

### Sale Transaction Message
To send a sale transaction message, you need to create TransactionRequestMessage.
Parameters are optional based on the transaction message type.
For normal sale operation, you can just apply the amount or you can also apply the ECR Reference Number (POS app Id number if Applicable)
```c#
// minimum argument supply, charge $10.00
var requestMessage = new TransactionRequestMessage(10.00m);
// OR supply ECR reference number
var requestMessage = new TransactionRequestMessage(10.00m, "ECR0001");

// accept returning message, you need to cast to the correct response message
var responseMessage = (TransactionResponseMessage) await client.SendRequestAsync(requestMessage);

// then check for response code
if(responseMessage.ResponseCode == ResponseCodes.APPROVED)
{
    Console.WriteLine("Transaction APPROVED!");
    // You may keep these return values for future usages
    // Approval code from the host (bank or payment processor)
    Console.WriteLine(responseMessage.ApprovalCode);

    // Batch Number for the settlement requests
    Console.WriteLine(responseMessage.BatchNumber);

    // Terminal Reference Number for the VOID, REFUND, or other transaction
    Console.WriteLine(responseMessage.TerminalRefNo);

    // And the original amount
    Console.WriteLine(responseMessage.Amount);

    // You can find out other fields by intellisense
}

```

### VOID Transaction Message
To send a VOID transaction message, you need to create TransactionRequestMessage.
For this transaction type, you need to supply Terminal Reference Number (TerminalRefNo)
returned from the terminal in previous transaction and the transaction type.
```c#
// create VOID transaction message with the reference number to be voided
var requestMessage = new TransactionRequestMessage(
    transactionType = TransactionTypes.VOID, 
    terminalRefNo = "000001"
);

// accept returning message, you need to cast to the correct response message
var responseMessage = (TransactionResponseMessage) await client.SendRequestAsync(requestMessage);

// then check for response code
if(responseMessage.ResponseCode == ResponseCodes.APPROVED)
{
    Console.WriteLine("Transaction APPROVED!");
}

```
