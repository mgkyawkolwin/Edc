using System.Text;
using Edc.Client;
using Edc.Client.Transport;
using Edc.Core.Messages;

class Program
{
    static string ECRRefNo = "E";
    static string TerminalRefNo = "000000";
    static string ip = "192.168.1.105";
    static int port = 8888;
    static char continueChoice = 'Y';
    static EdcClient client;
    public static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("Sample Client started ...");
            Console.WriteLine("Please enter IP Address:");
            //string ip = Console.ReadLine() ?? "";
            Console.WriteLine("Please enter Port Number:");
            //int port = int.Parse(Console.ReadLine() ?? "0");

            ITransport transport = new TcpTransport(ip, port);
            client = new EdcClient(transport);
            Console.WriteLine("Connecting ...");
            await client.ConnectAsync();
            Console.WriteLine("Connected!");

            while (continueChoice == 'Y')
            {
                Console.WriteLine("Pleasa choose a transaction type:");
                Console.WriteLine("1 - SALE_FULL_PAYMENT");
                Console.WriteLine("2 - REFUND");
                string choice = Console.ReadLine() ?? "1";
                switch (choice)
                {
                    case "1":
                        await Program.SimulateFullPaymentAsync();
                        break;
                    case "2":
                        // Implement refund simulation here
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        continue;
                }

                Console.WriteLine("Do you want to send another request? (Y/N)");
                continueChoice = Char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    public static async Task SimulateFullPaymentAsync()
    {
        Console.WriteLine("Please enter amount:");
        decimal amount = decimal.Parse(Console.ReadLine() ?? "0");
        Console.WriteLine($"Simulating SALE_FULL_PAYMENT for amount: {amount}");
        TransactionRequestMessage requestMsg = new TransactionRequestMessage(ECRRefNo, TerminalRefNo, Edc.Core.Common.TransactionTypes.SALE_FULL_PAYMENT, amount);
        Console.WriteLine("Sending message ...");
        Console.WriteLine(BitConverter.ToString(requestMsg.GetMessage()));
        TransactionResponseMessage responseMsg = (TransactionResponseMessage)await client.SendRequestAsync(requestMsg);
        // if(responseMsg.IsValid())
        // {
        //     Console.WriteLine("Response LRC Verified");
        //     await client.SendRequestAsync(new AcknowledgeMessage()); // Send ACK
        // }
        // else
        // {
        //     Console.WriteLine("Response LRC Verification Failed");
        //     await client.SendRequestAsync(new NotAcknowledgeMessage()); // Send NAK
        // }
        Console.WriteLine("Response received!");
        Console.WriteLine("Response Code: " + Encoding.ASCII.GetString(responseMsg.GetResponseCode()));
        Console.WriteLine("Data Lenth: " + responseMsg.GetDataLength());
        Console.WriteLine(BitConverter.ToString(responseMsg.GetMessage()));
    }
}