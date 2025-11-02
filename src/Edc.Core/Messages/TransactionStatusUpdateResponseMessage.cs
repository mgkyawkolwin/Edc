using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class TransactionStatusUpdateResponseMessage : ResponseMessage
{
    public TransactionStatusUpdateResponseMessage(byte[] message)
    {
        _message = message;
    }

    public decimal Amount => Convert.ToDecimal(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.TransactionStatusUpdateMessage.Response.Amount, DataFieldLength.Amount)
        )
    );

    public string EcrRefNo => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionStatusUpdateMessage.Response.EcrRefNo, DataFieldLength.EcrRefNo)
    ).Trim();

    public string PrivateField => Encoding.ASCII.GetString(
        _message[DataFieldIndex.TransactionStatusUpdateMessage.Response.PrivateField..^3]
    );

    public override string ResponseCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionStatusUpdateMessage.Response.ResponseCode, DataFieldLength.ResponseCode)
    );
}