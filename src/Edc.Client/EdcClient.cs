using Edc.Client.Transport;
using Edc.Core.Common;
using Edc.Core.Exceptions;
using Edc.Core.Messages;
using Edc.Core.Utilities;
using Edc.Core.Factories;
using System.Reflection.Metadata.Ecma335;

namespace Edc.Client;

/// <summary>
/// Client for communicating with ISO-8583 supported EDC terminal devices.
/// Provides methods to send request messages and handle responses over a transport layer (TCP/IP currently).
/// </summary>
/// <remarks>
/// This client handles acknowledgment codes (ACK/NAK), transaction status updates, and final responses.
/// For SALE transactions, the terminal may send intermediate status updates (e.g., waiting for card swipe or PIN entry),
/// which can be handled via the <see cref="SendRequestAsync"/> <paramref name="onStatusUpdate"/> callback.
/// </remarks>
public class EdcClient : IEdcClient, IDisposable
{
    private readonly ITransport _transport;

    /// <summary>
    /// Initializes a new instance of <see cref="EdcClient"/> with the specified transport layer.
    /// </summary>
    /// <param name="transport">The transport layer to use for communication. Must implement <see cref="ITransport"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="transport"/> is null.</exception>
    public EdcClient(ITransport transport)
    {
        _transport = transport ?? throw new ArgumentNullException(nameof(transport));
    }

    /// <summary>
    /// Sends a request message to the EDC terminal and waits asynchronously for a response.
    /// </summary>
    /// <param name="requestMessage">The <see cref="RequestMessage"/> to send.</param>
    /// <param name="cancellationToken">Optional token to cancel the operation.</param>
    /// <param name="timeOutMs">Custom timeout in milliseconds to wait for the final response (default: <see cref="Constants.RESPONSE_TIMEOUT_MS"/>).</param>
    /// <param name="onStatusUpdate">
    /// Optional callback invoked when a <see cref="TransactionStatusUpdateResponseMessage"/> is received.
    /// This allows the caller to handle intermediate transaction steps, such as card swipe, PIN entry, or signature prompt.
    /// </param>
    /// <returns>
    /// A <see cref="ResponseMessage"/> representing the final response from the terminal.
    /// The caller may need to cast it to the appropriate response type.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="requestMessage"/> is null.</exception>
    /// <exception cref="NotAcknowledgedException">Thrown if the terminal does not acknowledge the request with an ACK.</exception>
    /// <exception cref="TimeoutException">Thrown if the final response is not received within the specified <paramref name="timeOutMs"/>.</exception>
    /// <remarks>
    /// This method performs the following steps:
    /// <list type="number">
    /// <item>
    /// <description>Sends the request message via the transport layer.</description>
    /// </item>
    /// <item>
    /// <description>Waits for a single-byte ACK/NAK response. If not acknowledged, a <see cref="NotAcknowledgedException"/> is thrown.</description>
    /// </item>
    /// <item>
    /// <description>Continuously reads data from the transport until a valid final response is received or the timeout expires.</description>
    /// </item>
    /// <item>
    /// <description>If intermediate status update messages (<see cref="TransactionStatusUpdateResponseMessage"/>) are received, 
    /// the <paramref name="onStatusUpdate"/> callback is invoked and the ACK/NAK is sent to the terminal.</description>
    /// </item>
    /// <item>
    /// <description>Validates the LRC (Longitudinal Redundancy Check) of each response and sends an ACK/NAK accordingly.</description>
    /// </item>
    /// <item>
    /// <description>Disconnects the transport after the final response or on timeout.</description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Receives a single-byte control code (ACK or NAK) from the terminal.
    /// </summary>
    /// <param name="timeoutMs">Timeout in milliseconds to wait for the control code.</param>
    /// <param name="cancellationToken">Optional token to cancel the operation.</param>
    /// <returns>The control code received (ACK or NAK), or 0x00 if none received within the timeout.</returns>
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

    /// <summary>
    /// Disposes the transport layer and releases any unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        _transport?.Dispose();
        GC.SuppressFinalize(this);
    }
}