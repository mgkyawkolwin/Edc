using Edc.Core.Common;

namespace Edc.Core.Messages;

public abstract class RequestMessage : BaseMessage
{
    protected RequestMessage()
    {
        SenderIndicator = Constants.SENDER_POS;
    }
    
}