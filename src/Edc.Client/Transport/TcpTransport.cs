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

    public bool IsConnected => _client?.Connected ?? false;

    public TcpTransport(string host, int port, int receiveTimeoutMs = Constants.RESPONSE_TIMEOUT_MS)
    {
        _host = host ?? throw new ArgumentNullException(nameof(host));
        _port = port;
        _receiveTimeoutMs = receiveTimeoutMs;
    }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Connecting to {_host}:{_port} ...");
        _client = new TcpClient();
        await _client.ConnectAsync(_host, _port);
        _stream = _client.GetStream();
        _stream.ReadTimeout = _receiveTimeoutMs;
        _stream.WriteTimeout = _receiveTimeoutMs;
    }

    public Task DisconnectAsync()
    {
        Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        try { _stream?.Close(); } catch { }
        try { _client?.Close(); } catch { }
        _stream = null;
        _client = null;
    }

    public async Task SendAsync(byte[] data, CancellationToken cancellationToken = default)
    {
        if (data == null || data.Length == 0) throw new ArgumentNullException(nameof(data));
        if(_client == null || !_client.Connected)
        {
            await ConnectAsync(cancellationToken);
        }

        await _stream.WriteAsync(data, cancellationToken);
        await _stream.FlushAsync(cancellationToken);
    }

    public async Task<byte[]> ReceiveAsync(int maxBytes = 4096, CancellationToken cancellationToken = default)
    {
        if (_client == null || !_client.Connected)
        {
            await ConnectAsync(cancellationToken);
        }

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
        catch (OperationCanceledException) { return new byte[] { }; }
        catch (Exception) { return new byte[] { }; }
    }
}