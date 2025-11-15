using Edc.Core.Common;

namespace Edc.Core.Messages;

/// <summary>
/// Represents the base class for all request messages sent from the POS to the EDC terminal.
/// Inherits common message functionality from <see cref="BaseMessage"/>.
/// </summary>
public abstract class RequestMessage : BaseMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestMessage"/> class.
    /// Sets the sender indicator to POS by default.
    /// </summary>
    protected RequestMessage()
    {
        SenderIndicator = Constants.SENDER_POS;
    }
}