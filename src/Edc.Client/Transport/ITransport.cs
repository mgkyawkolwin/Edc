
namespace Edc.Client.Transport;

public interface ITransport : IDisposable
{
    // Task ConnectAsync(CancellationToken cancellationToken = default);
    Task DisconnectAsync();
    Task SendAsync(byte[] data, CancellationToken cancellationToken = default);
    /// <summary>
    /// Waits for incoming bytes; returns array of bytes received (may be partial).
    /// Implementations should return empty array if nothing read within specified timeout.
    /// </summary>
    Task<byte[]> ReceiveAsync(int maxBytes = 4096, CancellationToken cancellationToken = default);
    // bool IsConnected { get; }
}
