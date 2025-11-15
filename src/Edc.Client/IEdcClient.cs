using Edc.Core.Common;
using Edc.Core.Messages;

namespace Edc.Client;

/// <summary>
/// Represents a client for communicating with an EDC terminal.
/// Provides methods to send requests and receive response messages asynchronously.
/// </summary>
public interface IEdcClient
{
    /// <summary>
    /// Sends a request message to the EDC terminal and asynchronously waits for a response.
    /// </summary>
    /// <param name="requestMessage">The <see cref="RequestMessage"/> to send.</param>
    /// <param name="cancellationToken">Optional token to cancel the operation.</param>
    /// <param name="timeOutMs">Timeout in milliseconds to wait for a response (default is <see cref="Constants.RESPONSE_TIMEOUT_MS"/>).</param>
    /// <param name="onStatusUpdate">
    /// Optional callback invoked when a <see cref="TransactionStatusUpdateResponseMessage"/> is received.
    /// This allows the caller to track transaction progress before the final response is returned.
    /// </param>
    /// <returns>
    /// A <see cref="Task{ResponseMessage}"/> representing the asynchronous operation.
    /// The task completes with the final response message from the terminal.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="requestMessage"/> is null.</exception>
    /// <exception cref="TimeoutException">Thrown if the response is not received within the specified <paramref name="timeOutMs"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the client is not connected or an error occurs while sending the request.</exception>
    Task<ResponseMessage> SendRequestAsync(
        RequestMessage requestMessage,
        CancellationToken cancellationToken = default,
        int timeOutMs = Constants.RESPONSE_TIMEOUT_MS,
        Action<TransactionStatusUpdateResponseMessage>? onStatusUpdate = null);
}