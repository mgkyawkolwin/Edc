using Edc.Core.Common;

namespace Edc.Core.Messages;

/// <summary>
/// Represents the base class for all response messages sent from the EDC terminal
/// back to the POS. Provides shared structure and behavior for terminal-originated messages.
/// </summary>
public abstract class ResponseMessage : BaseMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResponseMessage"/> class.
    /// Sets the sender indicator to the terminal by default.
    /// </summary>
    public ResponseMessage()
    {
        SenderIndicator = Constants.SENDER_TERMINAL;
    }

    /// <summary>
    /// Gets the response code provided by the terminal.
    /// Implementations must extract and return the code from the underlying message data.
    /// </summary>
    public abstract string ResponseCode { get; }
}