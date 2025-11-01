using Edc.Core.Common;

namespace Edc.Core.Messages;

public abstract class ResponseMessage : BaseMessage
{
    public ResponseMessage()
    {
        SenderIndicator = Constants.SENDER_TERMINAL;
    }

    public abstract string ResponseCode { get;}
}