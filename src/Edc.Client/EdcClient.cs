using Edc.Client.Transport;
using Edc.Core.Common;
using Edc.Core.Exceptions;
using Edc.Core.Messages;
using Edc.Core.Utilities;

namespace Edc.Client;

public class EdcClient : IEdcClient, IDisposable
{
    private readonly ITransport _transport;

    public EdcClient(ITransport transport)
    {
        _transport = transport ?? throw new ArgumentNullException(nameof(transport));
    }

    public async Task ConnectAsync(CancellationToken ct = default) => await _transport.ConnectAsync(ct);

    public async Task DisconnectAsync() => await _transport.DisconnectAsync();

    public async Task<ResponseMessage> SendRequestAsync(RequestMessage requestMessage, CancellationToken cancellationToken = default)
    {
        if (requestMessage == null) throw new ArgumentNullException(nameof(requestMessage));
        if (!_transport.IsConnected) throw new InvalidOperationException("Transport not connected");

        await _transport.SendAsync(requestMessage.GetMessage(), cancellationToken);

        var ack = await WaitForControlCodeAsync(Constants.ACK_TIMEOUT_MS, cancellationToken);
        if (ack != Constants.ACK) throw new NotAcknowledgedException();

        var sw = System.Diagnostics.Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < Constants.RESPONSE_TIMEOUT_MS)
        {
            var data = await _transport.ReceiveAsync(Constants.RESPONSE_BUFFER_SIZE, cancellationToken);
            if (data == null || data.Length == 0)
            {
                await Task.Delay(100, cancellationToken);
                continue;
            }

            var responseMessage = new TransactionResponseMessage(data);
            if (responseMessage.IsValid())
                await _transport.SendAsync([Constants.ACK], cancellationToken);
            else
                await _transport.SendAsync([Constants.NAK], cancellationToken);

            return responseMessage;
        }

        throw new TimeoutException("Timeout waiting for response");
    }

    private async Task<byte> WaitForControlCodeAsync(int timeoutMs, CancellationToken cancellationToken)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < timeoutMs)
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
    }
}
