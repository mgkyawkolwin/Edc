
namespace Edc.Client.Transport;

/// <summary>
/// Defines a contract for a transport layer used to communicate with an EDC terminal.
/// Provides methods for sending and receiving raw byte data and for managing the connection lifecycle.
/// </summary>
public interface ITransport : IDisposable
{
    /// <summary>
    /// Asynchronously disconnects from the transport endpoint.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous disconnect operation.</returns>
    Task DisconnectAsync();

    /// <summary>
    /// Asynchronously sends a byte array over the transport.
    /// </summary>
    /// <param name="data">The byte array to send.</param>
    /// <param name="cancellationToken">Optional token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous send operation.</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the transport has been disposed.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the transport is not connected.</exception>
    Task SendAsync(byte[] data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously receives bytes from the transport.
    /// </summary>
    /// <param name="maxBytes">The maximum number of bytes to read in a single call. Default is 4096.</param>
    /// <param name="cancellationToken">Optional token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="Task{Byte[]}"/> representing the asynchronous receive operation.
    /// Returns a byte array containing the data received, which may be a partial message.
    /// Returns an empty array if no data is received within the timeout.
    /// </returns>
    /// <remarks>
    /// Implementations should not block indefinitely. If no data is available within the configured
    /// timeout or before cancellation, an empty array should be returned.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">Thrown if the transport has been disposed.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the transport is not connected.</exception>
    Task<byte[]> ReceiveAsync(int maxBytes = 4096, CancellationToken cancellationToken = default);
}