namespace Edc.Core.Messages;

public abstract class ResponseMessage : BaseMessage
{
    public abstract byte[] GetResponseCode();
    public abstract bool IsValid();
}