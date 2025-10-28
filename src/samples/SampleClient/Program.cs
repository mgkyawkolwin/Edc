using System.Text;
using Edc.Client;
using Edc.Client.Transport;
using Edc.Core.Messages;

class Program
{
    static readonly string ECRRefNo = "E";
    static readonly string TerminalRefNo = "000000";
    static string ip = "192.168.3.150";
    static int port = 8888;
    static char continueChoice = 'Y';
    static EdcClient? client;
    public static async Task Main(string[] args)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Sample Client started ...");
            Console.WriteLine("Current EDC IP Address: " + ip);
            Console.WriteLine("Press Enter to use the current IP or type a new one:");
            var newIp = Console.ReadLine();
            if (!string.IsNullOrEmpty(newIp))
            {
                ip = newIp;
            }
            Console.WriteLine("Current EDC Port Number: " + port);
            Console.WriteLine("Press Enter to use the current Port or type a new onex:");
            var newPort = Console.ReadLine();
            if (!string.IsNullOrEmpty(newPort) && int.TryParse(newPort, out int parsedPort))
            {
                port = parsedPort;
            }

            ITransport transport = new TcpTransport(ip, port);
            client = new EdcClient(transport);
            Console.WriteLine($"Connecting {ip}:{port}...");
            await client.ConnectAsync();
            Console.WriteLine("Connected!");

            while (continueChoice != 'N')
            {
                try
                {
                    Console.WriteLine("Pleasa choose a transaction type:");
                    Console.WriteLine("1 - TEST CONNECTION");
                    Console.WriteLine("2 - CARD INQUIRY");
                    Console.WriteLine("3 - SALE FULL PAYMENT");
                    Console.WriteLine("0 - Exit Program");
                    string choice = Console.ReadLine() ?? "1";
                    switch (choice)
                    {
                        case "1":
                            await Program.TestConnection();
                            break;
                        case "2":
                            await Program.CardInquiryAsync();
                            break;
                        case "3":
                            await Program.SimulateFullPaymentAsync();
                            break;
                        case "0":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            continue;
                    }

                    Console.WriteLine("Do you want to send another request? (Y/N)");
                    continueChoice = Char.ToUpper(Console.ReadKey().KeyChar);
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static async Task TestConnection()
    {
        Console.WriteLine("Preparing to send TEST CONNECTION message...");
        var requestMsg = new ConnectionRequestMessage();
        Console.WriteLine("Data Length: " + requestMsg.DataLength);
        Console.WriteLine("POS DateTime: " + requestMsg.PosDateTime);
        Console.WriteLine("POS ID: " + requestMsg.PosID);
        Console.WriteLine("Request Message: ");
        Console.WriteLine(BitConverter.ToString(requestMsg.Message));
        if (client == null)
        {
            throw new Exception("Client is not initialized");
        }
        Console.WriteLine("Sending message ...");
        ConnectionResponseMessage responseMsg = (ConnectionResponseMessage)await client.SendRequestAsync(requestMsg);
        Console.WriteLine("RESPONSE");
        Console.WriteLine("Is Valid LRC: " + responseMsg.IsValidLRC());
        Console.WriteLine("Response Code: " + responseMsg.ResponseCode);
        Console.WriteLine("Data Length: " + responseMsg.DataLength);
        Console.WriteLine("POS DateTime: " + responseMsg.PosDateTime);
        Console.WriteLine("POS ID: " + responseMsg.PosID);
        Console.WriteLine("Response Message: ");
        Console.WriteLine(BitConverter.ToString(responseMsg.Message));
    }

    public static async Task CardInquiryAsync()
    {
        Console.WriteLine("Please enter amount:");
        decimal amount = decimal.Parse(Console.ReadLine() ?? "0");
        Console.WriteLine("Preparing to send CARD INQUIRY message...");
        var requestMsg = new CardInquiryRequestMessage(ECRRefNo, amount);
        Console.WriteLine("Data Length: " + requestMsg.DataLength);
        Console.WriteLine("Amount: " + requestMsg.Amount);
        Console.WriteLine("Request Message: ");
        Console.WriteLine(BitConverter.ToString(requestMsg.Message));
        if (client == null)
        {
            throw new Exception("Client is not initialized");
        }
        Console.WriteLine("Sending message ...");
        CardInquiryResponseMessage responseMsg = (CardInquiryResponseMessage)await client.SendRequestAsync(requestMsg);
        Console.WriteLine("RESPONSE");
        Console.WriteLine("Is Valid LRC: " + responseMsg.IsValidLRC());
        Console.WriteLine("Response Code: " + responseMsg.ResponseCode);
        Console.WriteLine("Data Length: " + responseMsg.DataLength);
        Console.WriteLine("Amount: " + responseMsg.Amount);
        Console.WriteLine("ApprovalCode: " + responseMsg.Amount);
        Console.WriteLine("EcrRefNo: " + responseMsg.EcrRefNo);
        Console.WriteLine("SenderIndicator: " + responseMsg.SenderIndicator);
        Console.WriteLine("TransactionType: " + responseMsg.TransactionType);
        Console.WriteLine("Response Message: ");
        Console.WriteLine(BitConverter.ToString(responseMsg.Message));
    }

    public static async Task SimulateFullPaymentAsync()
    {
        Console.WriteLine("Please enter amount:");
        decimal amount = decimal.Parse(Console.ReadLine() ?? "0");
        Console.WriteLine("Preparing to send TRANSACTION message...");
        var requestMsg = new TransactionRequestMessage(ECRRefNo, amount, Edc.Core.Common.TransactionTypes.SALE_FULL_PAYMENT);
        Console.WriteLine("Data Length: " + requestMsg.DataLength);
        Console.WriteLine("POS Amount: " + requestMsg.Amount);
        Console.WriteLine("Request Message: ");
        Console.WriteLine(BitConverter.ToString(requestMsg.Message));
        if (client == null)
        {
            throw new Exception("Client is not initialized");
        }
        Console.WriteLine("Sending message ...");
        TransactionResponseMessage responseMsg = (TransactionResponseMessage)await client.SendRequestAsync(requestMsg);
        Console.WriteLine("RESPONSE");
        Console.WriteLine("Is Valid LRC: " + responseMsg.IsValidLRC());
        Console.WriteLine("Response Code: " + responseMsg.ResponseCode);
        Console.WriteLine("Data Length: " + responseMsg.DataLength);
        Console.WriteLine("Amount: " + responseMsg.Amount);
        Console.WriteLine("ApprovalCode: " + responseMsg.ApprovalCode);
        Console.WriteLine("BatchNumber: " + responseMsg.BatchNumber);
        Console.WriteLine("CardExpDate: " + responseMsg.CardExpDate);
        Console.WriteLine("CardLabel: " + responseMsg.CardLabel);
        Console.WriteLine("CardType: " + responseMsg.CardType);
        Console.WriteLine("DateTime: " + responseMsg.DateTime);
        Console.WriteLine("EcrRefNo: " + responseMsg.EcrRefNo);
        Console.WriteLine("EntryMode: " + responseMsg.EntryMode);
        Console.WriteLine("MerchantId: " + responseMsg.MerchantId);
        Console.WriteLine("NetAmount: " + responseMsg.NetAmount);
        Console.WriteLine("PAN: " + responseMsg.PAN);
        Console.WriteLine("PersonName: " + responseMsg.PersonName);
        Console.WriteLine("RedemptionAmount: " + responseMsg.RedemptionAmount);
        Console.WriteLine("RRN: " + responseMsg.RRN);
        Console.WriteLine("SenderIndicator: " + responseMsg.SenderIndicator);
        Console.WriteLine("SignatureNotRequiredIndicator: " + responseMsg.SignatureNotRequiredIndicator);
        Console.WriteLine("TerminalId: " + responseMsg.TerminalId);
        Console.WriteLine("PTerminalRefNo: " + responseMsg.TerminalRefNo);
        Console.WriteLine("TransactionType: " + responseMsg.TransactionType);
        Console.WriteLine("Response Message: ");
        Console.WriteLine(BitConverter.ToString(responseMsg.Message));
    }
}