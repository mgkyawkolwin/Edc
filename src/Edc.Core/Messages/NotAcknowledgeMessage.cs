

using Edc.Core.Common;

namespace Edc.Core.Messages;

public class NotAcknowledgeMessage : RequestMessage
{
    public override byte[] GetData()
    {
        throw new NotImplementedException();
    }

    public override int GetDataLength()
    {
        throw new NotImplementedException();
    }

    public override byte[] GetMessage()
    {
        return [Constants.ACK];;
    }
}