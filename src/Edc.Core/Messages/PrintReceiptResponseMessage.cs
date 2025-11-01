using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class PrintReceiptResponseMessage : ResponseMessage
{
    // private readonly byte[] _data;
    public PrintReceiptResponseMessage(byte[] message)
    {
        _message = message;
    }

    public string PosDateTime => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.ConnectionMessage.Response.PosDateTime , DataFieldLength.PosDateTime)
    );

    public string PosID => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.ConnectionMessage.Response.PosID , DataFieldLength.PosID)
    );

    public override string ResponseCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.ConnectionMessage.Response.ResponseCode , DataFieldLength.ResponseCode)
    );
}