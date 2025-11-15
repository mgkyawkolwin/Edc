using System.Net.Sockets;
using Edc.Core.Common;

namespace Edc.Client.Transport;

/// <summary>
/// Implementation of <see cref="ITransport"/> that communicates with an EDC terminal over TCP.
/// Handles connection management, sending, and receiving raw byte data with configurable timeouts.
/// </summary>
public class TcpTransport : ITransport
{
    private readonly string _host;
    private readonly int _port;
    private TcpClient? _client;
    private NetworkStream? _stream;
    private readonly int _receiveTimeoutMs;

    /// <summary>
    /// Initializes a new instance of <see cref="TcpTransport"/> with the specified host, port, and receive timeout.
    /// </summary>
    /// <param name="host">The hostname or IP address of the EDC terminal.</param>
    /// <param name="port">The TCP port to connect to.</param>
    /// <param name="receiveTimeoutMs">The receive timeout in milliseconds (default is <see cref="Constants.RESPONSE_TIMEOUT_MS"/>).</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="host"/> is null.</exception>
    public TcpTransport(string host, int port, int receiveTimeoutMs = Constants.RESPONSE_TIMEOUT_MS)
    {
        _host = host ?? throw new ArgumentNullException(nameof(host));
        _port = port;
        _receiveTimeoutMs = receiveTimeoutMs;
    }

    /// <summary>
    /// Establishes a TCP connection to the terminal if not already connected.
    /// </summary>
    /// <param name="cancellationToken">Optional token to cancel the connection attempt.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous connect operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the connection could not be established.</exception>
    private async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        _client ??= new TcpClient();
        if (!_client.Connected) await _client.ConnectAsync(_host, _port, cancellationToken);
        if (!_client.Connected) throw new InvalidOperationException("Could not connect to the terminal.");
        _stream ??= _client.GetStream();
        _stream.ReadTimeout = _receiveTimeoutMs;
        _stream.WriteTimeout = _receiveTimeoutMs;
    }

    /// <inheritdoc/>
    public Task DisconnectAsync()
    {
        _stream?.Dispose();
        _client?.Dispose();
        _stream = null;
        _client = null;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _stream?.Dispose();
        _client?.Dispose();
        _stream = null;
        _client = null;
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if connection cannot be established or network stream is unavailable.</exception>
    public async Task SendAsync(byte[] data, CancellationToken cancellationToken = default)
    {
        if (data == null || data.Length == 0) throw new ArgumentNullException(nameof(data));
        if (_client == null || !_client.Connected) await ConnectAsync(cancellationToken);

        if (_stream == null) throw new InvalidOperationException("Could not get network stream from TCP client");

        await _stream.WriteAsync(data, cancellationToken);
        await _stream.FlushAsync(cancellationToken);
    }

    /// <inheritdoc/>
    /// <param name="maxBytes">Maximum number of bytes to read in a single operation (default 4096).</param>
    /// <returns>
    /// A byte array containing the received data. 
    /// May be partial if fewer bytes are available. Returns an empty array if no data is read within the timeout.
    /// </returns>
    /// <remarks>
    /// This method handles timeouts and disconnections. If the read operation exceeds the timeout,
    /// the connection is automatically disconnected and an empty array is returned.
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown if the connection is lost during reading or an unexpected read error occurs.</exception>
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