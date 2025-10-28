using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class TransactionResponseMessage : ResponseMessage
{
    public TransactionResponseMessage(byte[] message)
    {
        _message = message;
    }

    public string ApprovalCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.ApprovalCode, DataFieldLength.ApprovalCode)
    );

    public decimal Amount => Convert.ToDecimal(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.TransactionMessage.Response.Amount, DataFieldLength.Amount)
        )
    );

    public string BatchNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.BatchNumber, DataFieldLength.BatchNumber)
    );

    public string CardExpDate => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.CardExpDate, DataFieldLength.CardExpDate)
    );

    public string CardLabel => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.CardLabel, DataFieldLength.CardLabel)
    );

    public string CardType => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.CardType, DataFieldLength.CardType)
    );

    public string DateTime => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.DateTime, DataFieldLength.DateTime)
    );

    public string EcrRefNo => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.EcrRefNo, DataFieldLength.EcrRefNo)
    );

    public string EntryMode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.EntryMode, DataFieldLength.EntryMode)
    );

    public string MerchantId => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.MerchantId, DataFieldLength.MerchantId)
    );

    public string PAN => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.PAN, DataFieldLength.PAN)
    );

    public string PersonName => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.PersonName, DataFieldLength.PersonName)
    );

    public string RedemptionAmount => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.RedemptionAmount, DataFieldLength.RedemptionAmount)
    );

    public string RRN => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.RRN, DataFieldLength.RRN)
    );

    public string NetAmount => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.NetAmount, DataFieldLength.NetAmount)
    );

    public string SignatureNotRequiredIndicator => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.SignatureNotRequiredIndicator, DataFieldLength.SignatureNotRequiredIndicator)
    );

    public string TerminalId => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.TerminalId, DataFieldLength.TerminalId)
    );

    public string TerminalRefNo => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.TerminalRefNo, DataFieldLength.TerminalRefNo)
    );

    public byte[] PrivateField => _message[DataFieldIndex.TransactionMessage.Response.PrivateField..^3];

    public override string ResponseCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.ResponseCode, DataFieldLength.ResponseCode)
    );
}