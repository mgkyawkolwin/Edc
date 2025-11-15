using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    private static TcpListener _listener;
    private static bool _isRunning = true;
    private static Dictionary<string, byte[]> _lastTransactionResponses = new Dictionary<string, byte[]>();

    static async Task Main(string[] args)
    {
        Console.WriteLine("EDC Terminal Interface Server Starting...");
        Console.WriteLine("Listening on port 8888");
        Console.WriteLine("Response Code Simulation:");
        Console.WriteLine("Last 2 digits of amount determine response:");
        Console.WriteLine("00 = Success, 01-99 = Various error codes");

        try
        {
            _listener = new TcpListener(IPAddress.Any, 8888);
            _listener.Start();

            while (_isRunning)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClient(client));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Server error: {ex.Message}");
        }
        finally
        {
            _listener?.Stop();
        }
    }

    private static async Task HandleClient(TcpClient client)
    {
        Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");

        try
        {
            using (client)
            using (var stream = client.GetStream())
            {
                var buffer = new byte[1024];

                while (client.Connected)
                {
                    var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    var requestData = new byte[bytesRead];
                    Array.Copy(buffer, requestData, bytesRead);

                    Console.WriteLine($"Received {bytesRead} bytes from client");
                    Console.WriteLine($"Hex: {BitConverter.ToString(requestData)}");

                    // Validate LRC
                    if (!ValidateLRC(requestData))
                    {
                        Console.WriteLine("LRC validation failed, sending NAK");
                        await stream.WriteAsync(new byte[] { 0x15 }, 0, 1); // NAK
                        continue;
                    }

                    // Send ACK for valid message
                    await stream.WriteAsync(new byte[] { 0x06 }, 0, 1); // ACK

                    // Process the request and generate response
                    var responseData = ProcessRequest(requestData);

                    if (responseData != null)
                    {
                        Console.WriteLine($"Sending {responseData.Length} bytes response");
                        Console.WriteLine($"Hex: {BitConverter.ToString(responseData)}");

                        await stream.WriteAsync(responseData, 0, responseData.Length);

                        // Wait for ACK from client
                        var ackBuffer = new byte[1];
                        var ackRead = await stream.ReadAsync(ackBuffer, 0, 1);
                        if (ackRead > 0 && ackBuffer[0] != (byte)0x06)
                        {
                            Console.WriteLine("Client didn't send ACK for response");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
        }

        Console.WriteLine($"Client disconnected: {client.Client.RemoteEndPoint}");
    }

    private static bool ValidateLRC(byte[] data)
    {
        Console.WriteLine("Validating LRC...");
        if (data.Length < 3) return false;
        Console.WriteLine("Data Length: " + data.Length.ToString());
        if (data[0] != 0x02) return false; // STX
        Console.WriteLine("STX Verified");

        byte lrc = 0;
        for (int i = 1; i < data.Length - 1; i++)
        {
            lrc ^= data[i];
        }

        Console.WriteLine("Server calculated LRC: " + lrc.ToString("X2"));

        return lrc == data[data.Length - 1];
    }

    private static byte[] ProcessRequest(byte[] request)
    {
        if (request.Length < 6) return null;

        var senderIndicator = (char)request[3];
        var transactionType = request[4];

        // Extract ECR Reference Number for duplicate checking
        string ecrRef = Encoding.ASCII.GetString(request, 7, 20).Trim();

        // Check for duplicate transaction
        if (_lastTransactionResponses.ContainsKey(ecrRef) &&
            IsDuplicateTransaction(request, _lastTransactionResponses[ecrRef]))
        {
            Console.WriteLine("Duplicate transaction detected, returning last response");
            //return _lastTransactionResponses[ecrRef];
        }

        byte[] response = null;

        switch (transactionType)
        {
            case 0x30: // Sale - Full Payment
            case 0x3A: // Sale Loyalty with Redemption
            case 0x41: // Alipay Sale
            case 0x42: // WeChat Pay Sale
            case 0x43: // UPI QR Sale
            case 0x4E: // PayNow Sale
                response = CreateSaleResponse(request);
                break;

            case 0x50: // Card Enquiry
            case 0x51: // Card Enquiry before Sales
                response = CreateCardEnquiryResponse(request);
                break;

            case 0x53: // Settlement
                response = CreateSettlementResponse(request);
                break;

            case 0x63: // Connection Test
                response = CreateConnectionTestResponse(request);
                break;

            case 0x52: // Print Settlement Report
            case 0x54: // Print Summary Report  
            case 0x55: // Print Detail Report
            case 0x56: // Reprint receipt
                response = CreatePrintResponse(request);
                break;

            default:
                response = CreateErrorResponse(request, "TA"); // Transaction Aborted
                break;
        }

        // Store last response for duplicate handling
        if (response != null && !string.IsNullOrEmpty(ecrRef))
        {
            _lastTransactionResponses[ecrRef] = response;
        }

        return response;
    }

    private static bool IsDuplicateTransaction(byte[] currentRequest, byte[] lastResponse)
    {
        // Simple duplicate check - in real implementation, you'd compare more fields
        return true; // For demo purposes
    }

    private static string GetResponseCodeFromAmount(byte[] amountField)
    {
        // Amount field is 12 bytes numeric ASCII, last 2 digits are cents
        // Example: "000000001002" = $10.02 -> response code "00" (success)
        // Example: "000000001005" = $10.05 -> response code "05" (declined)

        if (amountField.Length != 12) return "00";

        string amountStr = Encoding.ASCII.GetString(amountField);
        string lastTwoDigits = amountStr.Substring(10, 2); // Last 2 digits (cents)

        Console.WriteLine($"Amount: {amountStr}, Response Code: {lastTwoDigits}");

        return lastTwoDigits;
    }

    private static string GetResponseMessage(string responseCode)
    {
        // Map response codes to meaningful messages
        var responseMessages = new Dictionary<string, string>
            {
                {"00", "Transaction Approved"},
                {"01", "Refer to Issuer"},
                {"02", "Refer to Issuer, Special"},
                {"03", "Invalid Merchant"},
                {"04", "Pickup Card"},
                {"05", "Do Not Honor"},
                {"07", "Pickup Card, Special"},
                {"12", "Invalid Transaction"},
                {"13", "Invalid Amount"},
                {"14", "Invalid Card Number"},
                {"15", "No Such Issuer"},
                {"19", "Re-enter Transaction"},
                {"25", "Unable to Locate Record"},
                {"30", "Format Error"},
                {"33", "Expired Card"},
                {"34", "Suspected Fraud"},
                {"38", "Allowable PIN Tries Exceeded"},
                {"41", "Lost Card"},
                {"43", "Stolen Card"},
                {"51", "Insufficient Funds"},
                {"54", "Expired Card"},
                {"55", "Incorrect PIN"},
                {"57", "Transaction Not Permitted"},
                {"58", "Transaction Not Permitted to Terminal"},
                {"59", "Suspected Fraud"},
                {"61", "Exceeds Withdrawal Limit"},
                {"62", "Restricted Card"},
                {"63", "Security Violation"},
                {"65", "Exceeds Withdrawal Frequency"},
                {"75", "Allowable PIN Tries Exceeded"},
                {"76", "Invalid Account"},
                {"77", "Invalid Amount"},
                {"78", "No Account"},
                {"79", "Already Reversed"},
                {"80", "No Financial Impact"},
                {"81", "Cryptographic Error"},
                {"82", "CVV Validation"},
                {"83", "Unable to Verify PIN"},
                {"84", "Invalid Life Cycle"},
                {"85", "No Reason to Decline"},
                {"86", "Cannot Verify PIN"},
                {"87", "Purchase Amount Only, No Cash Back"},
                {"88", "Cryptographic Failure"},
                {"89", "Authentication Failure"},
                {"91", "Issuer or Switch Inoperative"},
                {"92", "Financial Institution Not Found"},
                {"93", "Transaction Cannot Be Completed"},
                {"94", "Duplicate Transmission"},
                {"95", "Reconcile Error"},
                {"96", "System Malfunction"},
                {"97", "No Funds Transfer"},
                {"98", "Duplicate Reversal"},
                {"99", "Duplicate Transaction"}
            };

        return responseMessages.ContainsKey(responseCode) ?
            responseMessages[responseCode] : $"Declined - Code {responseCode}";
    }

    private static byte[] CreateSaleResponse(byte[] request)
    {
        var response = new List<byte>();

        // Extract amount from request (bytes 27-38)
        byte[] amountField = new byte[12];
        Array.Copy(request, 28, amountField, 0, 12);

        // Determine response code from last 2 digits of amount
        string responseCode = GetResponseCodeFromAmount(amountField);
        string responseMessage = GetResponseMessage(responseCode);

        Console.WriteLine($"Transaction Response: {responseCode} - {responseMessage}");

        // STX
        response.Add(0x02);

        // Data Length - will be calculated later
        response.Add(0x00);
        response.Add(0x00);

        // Sender Indicator
        response.Add((byte)'T');

        // Transaction Type (same as request)
        response.Add(request[4]);

        // Message Version
        response.AddRange(Encoding.ASCII.GetBytes("V18"));

        // ECR Reference No (same as request)
        response.AddRange(new ArraySegment<byte>(request, 8, 20));

        // Amount (same as request)
        response.AddRange(amountField);

        // Response Code - determined from amount
        response.AddRange(Encoding.ASCII.GetBytes(responseCode));

        // For non-00 responses, send shorter response
        if (responseCode != "00")
        {
            // ETX
            response.Add(0x03);

            // Calculate and set data length
            int dLength = response.Count - 4;
            response[1] = (byte)((dLength >> 8) & 0xFF);
            response[2] = (byte)(dLength & 0xFF);

            // Calculate LRC
            byte lrcc = 0;
            for (int i = 1; i < response.Count - 1; i++)
            {
                lrcc ^= response[i];
            }
            response.Add(lrcc);

            return response.ToArray();
        }

        // Full response for approved transactions (00)

        // Merchant ID
        response.AddRange(Encoding.ASCII.GetBytes("123456789012345"));

        // Terminal ID  
        response.AddRange(Encoding.ASCII.GetBytes("88888888"));

        // PAN (masked)
        response.AddRange(Encoding.ASCII.GetBytes("XXXXXXXXXXXX9012\0\0\0\0"));

        // Card Exp Date (masked)
        response.AddRange(Encoding.ASCII.GetBytes("XXXX"));

        // Approval Code
        response.AddRange(Encoding.ASCII.GetBytes("123456"));

        // Card Label
        response.AddRange(Encoding.ASCII.GetBytes("VISA\0\0\0\0\0\0"));

        // RRN
        response.AddRange(Encoding.ASCII.GetBytes("123456789012"));

        // Date/Time
        response.AddRange(Encoding.ASCII.GetBytes(DateTime.Now.ToString("yyMMddHHmmss")));

        // Batch Number
        response.AddRange(Encoding.ASCII.GetBytes("000001"));

        // Card Type
        response.AddRange(Encoding.ASCII.GetBytes("VI"));

        // Person's name
        response.AddRange(Encoding.ASCII.GetBytes("TEST CUSTOMER\0\0\0\0\0\0\0\0\0\0\0\0\0\0"));

        // Signature Not Required Indicator
        response.Add((byte)'0');

        // Entry Mode
        response.AddRange(Encoding.ASCII.GetBytes("07")); // Contactless chip

        // Terminal Ref Number
        response.AddRange(Encoding.ASCII.GetBytes("000123"));

        // Redemption Amount
        response.AddRange(Encoding.ASCII.GetBytes("000000000000"));

        // Net Amount
        response.AddRange(amountField); // Same as original amount

        // Add response text in private field for non-00 responses
        if (responseCode != "00")
        {
            var privateField = CreatePrivateFieldWithResponseText(responseMessage);
            response.AddRange(privateField);
        }

        // ETX
        response.Add(0x03);

        // Calculate and set data length
        int dataLength = response.Count - 4; // Exclude STX (1), Length (2), ETX (1)
        response[1] = (byte)((dataLength >> 8) & 0xFF);
        response[2] = (byte)(dataLength & 0xFF);

        // Calculate LRC
        byte lrc = 0;
        for (int i = 1; i < response.Count - 2; i++)
        {
            lrc ^= response[i];
        }
        response.Add(lrc);

        return response.ToArray();
    }

    private static byte[] CreatePrivateFieldWithResponseText(string responseText)
    {
        var pf = new List<byte>();

        // Private Field Tag
        pf.AddRange(Encoding.ASCII.GetBytes("PF"));

        // Response Text Tag
        var rtTag = new List<byte>();
        rtTag.AddRange(Encoding.ASCII.GetBytes("RT"));

        string rtLength = responseText.Length.ToString("D3");
        rtTag.AddRange(Encoding.ASCII.GetBytes(rtLength));
        rtTag.AddRange(Encoding.ASCII.GetBytes(responseText));

        // Set PF length
        string pfLength = rtTag.Count.ToString("D3");
        pf.AddRange(Encoding.ASCII.GetBytes(pfLength));
        pf.AddRange(rtTag);

        return pf.ToArray();
    }

    private static byte[] CreateCardEnquiryResponse(byte[] request)
    {
        var response = new List<byte>();

        // STX
        response.Add(0x02);

        // Data Length - temporary
        response.Add(0x00);
        response.Add(0x00);

        // Sender Indicator
        response.Add((byte)'T');

        // Transaction Type
        response.Add(request[4]);

        // Message Version
        response.AddRange(Encoding.ASCII.GetBytes("V19"));

        // ECR Reference No
        response.AddRange(new ArraySegment<byte>(request, 7, 20));

        // Amount
        response.AddRange(new ArraySegment<byte>(request, 27, 12));

        // Response Code - Card enquiry usually succeeds
        response.AddRange(Encoding.ASCII.GetBytes("00"));

        // Private Field with card data
        var privateField = CreateCardEnquiryPrivateField();
        response.AddRange(privateField);

        // ETX
        response.Add(0x03);

        // Calculate and set data length
        int dataLength = response.Count - 4;
        response[1] = (byte)((dataLength >> 8) & 0xFF);
        response[2] = (byte)(dataLength & 0xFF);

        // Calculate LRC
        byte lrc = 0;
        for (int i = 1; i < response.Count - 1; i++)
        {
            lrc ^= response[i];
        }
        response.Add(lrc);

        return response.ToArray();
    }

    private static byte[] CreateCardEnquiryPrivateField()
    {
        var pf = new List<byte>();

        // Private Field Tag
        pf.AddRange(Encoding.ASCII.GetBytes("PF"));

        // Calculate total length of sub-fields
        var subFields = new List<byte>();

        // CH - SHA-512 hash of card number
        subFields.AddRange(Encoding.ASCII.GetBytes("CH128"));
        subFields.AddRange(Encoding.ASCII.GetBytes("0176BA4117BBD81FDC6B508A562D97506637FB192CXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"));

        // CT - Card Type
        subFields.AddRange(Encoding.ASCII.GetBytes("CT002VI"));

        // CL - Card Label
        subFields.AddRange(Encoding.ASCII.GetBytes("CL004VISA"));

        // CM - Masked Card Number
        subFields.AddRange(Encoding.ASCII.GetBytes("CM016XXXXXXXXXXXX9012"));

        // EM - Entry Mode
        subFields.AddRange(Encoding.ASCII.GetBytes("EM001C"));

        // PN - Cardholder Name
        subFields.AddRange(Encoding.ASCII.GetBytes("PN026TEST CUSTOMER NAME HERE"));

        // Set PF length
        string pfLength = subFields.Count.ToString("D3");
        pf.AddRange(Encoding.ASCII.GetBytes(pfLength));
        pf.AddRange(subFields);

        return pf.ToArray();
    }

    private static byte[] CreateSettlementResponse(byte[] request)
    {
        var response = new List<byte>();

        // STX
        response.Add(0x02);

        // Data Length - temporary
        response.Add(0x00);
        response.Add(0x00);

        // Sender Indicator
        response.Add((byte)'T');

        // Transaction Type
        response.Add(request[4]);

        // Message Version
        response.AddRange(Encoding.ASCII.GetBytes("V18"));

        // POS Date/Time
        response.AddRange(new ArraySegment<byte>(request, 7, 14));

        // POS ID
        response.AddRange(new ArraySegment<byte>(request, 21, 6));

        // Host Number
        response.AddRange(new ArraySegment<byte>(request, 27, 3));

        // Reserved
        response.AddRange(Encoding.ASCII.GetBytes("000000000"));

        // Response Code - Settlement usually succeeds
        response.AddRange(Encoding.ASCII.GetBytes("00"));

        // Settlement Summary
        var settlementSummary = CreateSettlementSummary();
        response.AddRange(settlementSummary);

        // ETX
        response.Add(0x03);

        // Calculate and set data length
        int dataLength = response.Count - 4;
        response[1] = (byte)((dataLength >> 8) & 0xFF);
        response[2] = (byte)(dataLength & 0xFF);

        // Calculate LRC
        byte lrc = 0;
        for (int i = 1; i < response.Count - 1; i++)
        {
            lrc ^= response[i];
        }
        response.Add(lrc);

        return response.ToArray();
    }

    private static byte[] CreateSettlementSummary()
    {
        var summary = new List<byte>();

        // Host Count
        summary.AddRange(Encoding.ASCII.GetBytes("02"));

        // First Host
        summary.AddRange(Encoding.ASCII.GetBytes("01")); // Host Number
        summary.AddRange(Encoding.ASCII.GetBytes("VISA     ")); // Host Name
        summary.Add((byte)'0'); // Settlement Result (0=ok)
        summary.AddRange(Encoding.ASCII.GetBytes("015")); // Sale Count
        summary.AddRange(Encoding.ASCII.GetBytes("000000015000")); // Sale Amount
        summary.AddRange(Encoding.ASCII.GetBytes("002")); // Refund Count  
        summary.AddRange(Encoding.ASCII.GetBytes("000000002500")); // Refund Amount

        // Second Host
        summary.AddRange(Encoding.ASCII.GetBytes("02")); // Host Number
        summary.AddRange(Encoding.ASCII.GetBytes("AMEX     ")); // Host Name
        summary.Add((byte)'0'); // Settlement Result (0=ok)
        summary.AddRange(Encoding.ASCII.GetBytes("003")); // Sale Count
        summary.AddRange(Encoding.ASCII.GetBytes("000000003000")); // Sale Amount
        summary.AddRange(Encoding.ASCII.GetBytes("001")); // Refund Count
        summary.AddRange(Encoding.ASCII.GetBytes("000000001000")); // Refund Amount

        return summary.ToArray();
    }

    private static byte[] CreateConnectionTestResponse(byte[] request)
    {
        var response = new List<byte>();

        // STX
        response.Add(0x02);

        // Data Length
        response.Add(0x00);
        response.Add(0x39); // 57 bytes data

        // Sender Indicator
        response.Add((byte)'T');

        // Transaction Type
        response.Add(request[4]);

        // Message Version
        response.AddRange(new ArraySegment<byte>(request, 5, 3));

        // POS Date/Time
        response.AddRange(new ArraySegment<byte>(request, 8, 14));

        // POS ID
        response.AddRange(new ArraySegment<byte>(request, 22, 6));

        // Reserved
        response.AddRange(Encoding.ASCII.GetBytes("            ")); // 12 spaces

        // Response Code - Connection test always succeeds
        response.AddRange(Encoding.ASCII.GetBytes("00"));

        // ETX
        response.Add(0x03);

        // Calculate LRC
        byte lrc = 0;
        for (int i = 1; i <= response.Count - 1; i++)
        {
            lrc ^= response[i];
        }
        response.Add(lrc);

        return response.ToArray();
    }

    private static byte[] CreatePrintResponse(byte[] request)
    {
        var response = new List<byte>();

        // STX
        response.Add(0x02);

        // Data Length - temporary
        response.Add(0x00);
        response.Add(0x00);

        // Sender Indicator
        response.Add((byte)'T');

        // Transaction Type
        response.Add(request[4]);

        // Message Version
        response.AddRange(Encoding.ASCII.GetBytes("V18"));

        // POS Date/Time
        response.AddRange(new ArraySegment<byte>(request, 7, 14));

        // POS ID
        response.AddRange(new ArraySegment<byte>(request, 21, 6));

        // Host Number
        response.AddRange(new ArraySegment<byte>(request, 27, 3));

        // Block Number (000000 for last block)
        response.AddRange(Encoding.ASCII.GetBytes("000000"));

        // Printing Data Length
        response.AddRange(Encoding.ASCII.GetBytes("035")); // 35 characters

        // Response Code
        response.AddRange(Encoding.ASCII.GetBytes("00"));

        // Printing Data
        var printData = Encoding.ASCII.GetBytes("SETTLEMENT REPORT SUCCESSFUL\nThank you!");
        response.AddRange(printData);

        // ETX
        response.Add(0x03);

        // Calculate and set data length
        int dataLength = response.Count - 4;
        response[1] = (byte)((dataLength >> 8) & 0xFF);
        response[2] = (byte)(dataLength & 0xFF);

        // Calculate LRC
        byte lrc = 0;
        for (int i = 1; i < response.Count - 1; i++)
        {
            lrc ^= response[i];
        }
        response.Add(lrc);

        return response.ToArray();
    }

    private static byte[] CreateErrorResponse(byte[] request, string errorCode)
    {
        var response = new List<byte>();

        // STX
        response.Add(0x02);

        // Data Length - temporary
        response.Add(0x00);
        response.Add(0x00);

        // Sender Indicator
        response.Add((byte)'T');

        // Transaction Type
        response.Add(request[4]);

        // Message Version
        response.AddRange(new ArraySegment<byte>(request, 7, 3));

        // ECR Reference No
        response.AddRange(new ArraySegment<byte>(request, 10, 20));

        // Amount
        response.AddRange(new ArraySegment<byte>(request, 30, 12));

        // Error Response Code
        response.AddRange(Encoding.ASCII.GetBytes(errorCode));

        // ETX
        response.Add(0x03);

        // Calculate and set data length
        int dataLength = response.Count - 4;
        response[1] = (byte)((dataLength >> 8) & 0xFF);
        response[2] = (byte)(dataLength & 0xFF);

        // Calculate LRC
        byte lrc = 0;
        for (int i = 1; i < response.Count - 1; i++)
        {
            lrc ^= response[i];
        }
        response.Add(lrc);

        return response.ToArray();
    }
}
