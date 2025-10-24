using Edc.Client;
using Edc.Client.Transport;
using Edc.Core.Messages;

class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("Client started ...");
            ITransport transport = new TcpTransport("193.168.3.150", 8888);
            EdcClient client = new EdcClient(transport);
            Console.WriteLine("Connecting ...");
            await client.ConnectAsync();
            Console.WriteLine("Connected!");

            TransactionRequestMessage requestMsg = new TransactionRequestMessage("ercRefNo", "terminalRefNo", Edc.Core.Common.TransactionTypes.SALE_FULL_PAYMENT, 123.00M);
            Console.WriteLine("Sending message ...");
            var response = await client.SendRequestAsync(requestMsg);
            Console.WriteLine("Response received!");
            Console.WriteLine(response.DataLength);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }



    }
}