using Edc.Client.Transport;
using Edc.Core.Common;
using Edc.Core.Exceptions;
using Edc.Core.Messages;
using Edc.Core.Utilities;
using Edc.Core.Factories;
using System.Reflection.Metadata.Ecma335;

namespace Edc.Client;

public class EdcClient : IEdcClient, IDisposable
{
    private readonly ITransport _transport;

    public EdcClient(ITransport transport)
    {
        _transport = transport ?? throw new ArgumentNullException(nameof(transport));
    }

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
