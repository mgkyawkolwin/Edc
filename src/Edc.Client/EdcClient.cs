using Edc.Client.Transport;
using Edc.Core.Common;
using Edc.Core.Exceptions;
using Edc.Core.Messages;
using Edc.Core.Utilities;
using Edc.Core.Factories;
using System.Reflection.Metadata.Ecma335;

namespace Edc.Client;

/// <summary>
/// The client to connect to ISO-8583 supproted EDC terminal devices. 
/// </summary>
public class EdcClient : IEdcClient, IDisposable
{
    private readonly ITransport _transport;

    /// <summary> 
    /// The transport layer, currently supported TCP/IP connection.
    /// You can extend to implement Serial Communication.
    /// </summary>
    /// <param name="transport"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public EdcClient(ITransport transport)
    {
        _transport = transport ?? throw new ArgumentNullException(nameof(transport));
    }

    /// <summary>
    /// The only function that client apps need to use.
    /// </summary>
    /// <param name="requestMessage">Message to be sent.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="timeOutMs">Custom timeout value in milliseconds.</param>
    /// <param name="onStatusUpdate">
    /// For SALE transactions, there are a few case that the terminal is waiting
    /// for the user input such as card swipe, key in, signature, etc.
    /// For those cases terminal will return the status update messages.
    /// Client can listen to this event and display corresponding message to notify user.
    /// </param>
    /// <returns>ResponseMessage - Need to cast to corresponding response message.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAcknowledgedException"></exception>
    /// <exception cref="TimeoutException"></exception>
    public async Task<ResponseMessage> SendRequestAsync(
        RequestMessage requestMessage,
        CancellationToken cancellationToken = default,
        int timeOutMs = Constants.RESPONSE_TIMEOUT_MS,
        Action<TransactionStatusUpdateResponseMessage>? onStatusUpdate = null)
    {
        byte acknowledgementResponse = 0x00;
        if (requestMessage == null) throw new ArgumentNullException(nameof(requestMessage));
        await _transport.SendAsync(requestMessage.Message, cancellationToken);
        acknowledgementResponse = await ReceiveControlCodeAsync(timeOutMs, cancellationToken);
        if (acknowledgementResponse != Constants.ACK)
        {
            await _transport.DisconnectAsync();
            throw new NotAcknowledgedException();
        }


        var sw = System.Diagnostics.Stopwatch.StartNew();
        while (timeOutMs == 0 || sw.ElapsedMilliseconds < timeOutMs)
        {
            var data = await _transport.ReceiveAsync(Constants.RESPONSE_BUFFER_SIZE, cancellationToken);
            Console.WriteLine("Data Response Received:");
            Console.WriteLine(BitConverter.ToString(data));
            if (data == null || data.Length == 0)
            {
                await Task.Delay(100, cancellationToken);
                continue;
            }

            var responseMessage = ResponseMessageFactory.CreateResponseMessage(data);
            if (responseMessage is TransactionStatusUpdateResponseMessage statusUpdate)
            {
                // Status update response received, return the message and wait for final response
                if (responseMessage.IsValidLRC())
                    await _transport.SendAsync(new byte[] { Constants.ACK }, cancellationToken);
                else
                    await _transport.SendAsync(new byte[] { Constants.NAK }, cancellationToken);
                onStatusUpdate?.Invoke(statusUpdate);
                sw.Restart();
                continue;
            }
            if (responseMessage.IsValidLRC())
                await _transport.SendAsync(new byte[] { Constants.ACK }, cancellationToken);
            else
                await _transport.SendAsync(new byte[] { Constants.NAK }, cancellationToken);
            await _transport.DisconnectAsync();
            return responseMessage;
        }

        await _transport.DisconnectAsync();
        throw new TimeoutException("Timeout waiting for response");
    }

    private async Task<byte> ReceiveControlCodeAsync(int timeoutMs, CancellationToken cancellationToken)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        while (timeoutMs == 0 || sw.ElapsedMilliseconds < timeoutMs)
        {
            var data = await _transport.ReceiveAsync(1, cancellationToken);
            Console.WriteLine("Acknowdedgement Response Received:");
            Console.WriteLine(BitConverter.ToString(data));
            if (data == null || data.Length == 0)
            {
                await Task.Delay(50, cancellationToken);
                continue;
            }
            if (data[0] == Constants.ACK || data[0] == Constants.NAK)
                return data[0];
        }
        return 0x00; // no control code
    }

    public void Dispose()
    {
        _transport?.Dispose();
        GC.SuppressFinalize(this);
    }
}
