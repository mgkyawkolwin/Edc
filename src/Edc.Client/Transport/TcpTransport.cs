using System.Net.Sockets;
using Edc.Core.Common;

namespace Edc.Client.Transport;

public class TcpTransport : ITransport
{
    private readonly string _host;
    private readonly int _port;
    private TcpClient? _client;
    private NetworkStream? _stream;
    private readonly int _receiveTimeoutMs;

    public TcpTransport(string host, int port, int receiveTimeoutMs = Constants.RESPONSE_TIMEOUT_MS)
    {
        _host = host ?? throw new ArgumentNullException(nameof(host));
        _port = port;
        _receiveTimeoutMs = receiveTimeoutMs;
    }

    private async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        _client ??= new TcpClient();
        if (!_client.Connected) await _client.ConnectAsync(_host, _port, cancellationToken);
        if (!_client.Connected) throw new InvalidOperationException("Could not connect to the terminal.");
        _stream ??= _client.GetStream();
        _stream.ReadTimeout = _receiveTimeoutMs;
        _stream.WriteTimeout = _receiveTimeoutMs;
    }

    public Task DisconnectAsync()
    {
        _stream?.Dispose();
        _client?.Dispose();
        _stream = null;
        _client = null;
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _stream?.Dispose();
        _client?.Dispose();
        _stream = null;
        _client = null;
        GC.SuppressFinalize(this);
    }

    public async Task SendAsync(byte[] data, CancellationToken cancellationToken = default)
    {
        if (data == null || data.Length == 0) throw new ArgumentNullException(nameof(data));
        if (_client == null || !_client.Connected) await ConnectAsync(cancellationToken);

        if (_stream == null) throw new InvalidOperationException("Could not get network stream from TCP client");

        await _stream.WriteAsync(data, cancellationToken);
        await _stream.FlushAsync(cancellationToken);
    }

    public async Task<byte[]> ReceiveAsync(int maxBytes = 4096, CancellationToken cancellationToken = default)
    {
        if (_client == null || !_client.Connected) await ConnectAsync(cancellationToken);

        if (_stream == null) throw new InvalidOperationException("Could not get network stream from TCP client");

        var buffer = new byte[maxBytes];
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(_receiveTimeoutMs);
        try
        {
            var read = await _stream.ReadAsync(buffer, 0, buffer.Length, cts.Token);
            if (read <= 0) return Array.Empty<byte>();
            if (read == buffer.Length) return buffer;
            var outBuf = new byte[read];
            Array.Copy(buffer, outBuf, read);
            return outBuf;
        }
        catch (OperationCanceledException)
        {
            await DisconnectAsync();
            return Array.Empty<byte>();
        }
        catch (IOException)
        {
            await DisconnectAsync();
            throw new InvalidOperationException("Connection lost during read.");
        }
        catch (Exception ex)
        {
            await DisconnectAsync();
            throw new InvalidOperationException("Unexpected read error.", ex);
        }
    }
}