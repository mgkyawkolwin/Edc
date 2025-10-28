using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class CardInquiryResponseMessage : ResponseMessage
{
    public CardInquiryResponseMessage(byte[] message)
    {
        _message = message;
    }

    public decimal Amount => Convert.ToDecimal(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.CardInquiryMessage.Response.Amount, DataFieldLength.Amount)
        )
    );

    public string EcrRefNo => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.CardInquiryMessage.Response.EcrRefNo, DataFieldLength.EcrRefNo)
    );

    public byte[] PrivateField => _message[DataFieldIndex.CardInquiryMessage.Response.PrivateField..^3];

    public override string ResponseCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.CardInquiryMessage.Response.ResponseCode, DataFieldLength.ResponseCode)
    );
}