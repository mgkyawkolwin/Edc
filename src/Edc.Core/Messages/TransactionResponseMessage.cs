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

    /// <summary>
    /// Approval code sent by the host (Not terminal's code).
    /// </summary>
    public string ApprovalCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.ApprovalCode, DataFieldLength.ApprovalCode)
    );

    /// <summary>
    /// Transaction amount, as sent in request message.
    /// </summary>
    public decimal Amount => Convert.ToDecimal(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.TransactionMessage.Response.Amount, DataFieldLength.Amount)
        )
    );

    /// <summary>
    /// Batch number from the terminal. Daily transactions are batched together. Later can be used to settle by settlement request message.
    /// </summary>
    public string BatchNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.BatchNumber, DataFieldLength.BatchNumber)
    );

    /// <summary>
    /// Card expiry date.
    /// </summary>
    public string CardExpDate => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.CardExpDate, DataFieldLength.CardExpDate)
    );

    /// <summary>
    /// Card label. Example, VISA, MASTER, JCB
    /// </summary>
    public string CardLabel
    {
        get
        {
            var span = _message.AsSpan(
                DataFieldIndex.TransactionMessage.Response.CardLabel,
                DataFieldLength.CardLabel
            );

            // Find null terminator (0x00)
            int nullIndex = span.IndexOf((byte)0x00);

            // Slice up to the null (or full length if none)
            var stringBytes = nullIndex >= 0 ? span[..nullIndex] : span;

            return Encoding.ASCII.GetString(stringBytes);
        }

    }

    /// <summary>
    /// Card type. Example, VI, MC, AM
    /// See Edc.Core.Common.CardTypes.cs for complete list.
    /// </summary>
    public string CardType => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.CardType, DataFieldLength.CardType)
    );

    /// <summary>
    /// DateTime sent by the ECR.
    /// </summary>
    public DateTime DateTime => Helper.GetDateTime(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.TransactionMessage.Response.DateTime, DataFieldLength.DateTime)
        )
    );

    /// <summary>
    /// Electronics Cash Register (ECR) reference number.
    /// Sent by the ECR.
    /// </summary>
    public string EcrRefNo => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.EcrRefNo, DataFieldLength.EcrRefNo)
    );

    /// <summary>
    /// Entry mode of the payment. Example, 05 - Chip/Insert, 07 - Contactless, 02 - Mag-Stripe.
    /// See Edc.Core.Common.EntryModes.cs for complete list.
    /// </summary>
    public string EntryMode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.EntryMode, DataFieldLength.EntryMode)
    );

    /// <summary>
    /// Merchant ID.
    /// </summary>
    public string MerchantId => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.MerchantId, DataFieldLength.MerchantId)
    );

    /// <summary>
    /// Personal account number.
    /// </summary>
    public string PAN 
    {
        get
        {
            var span = _message.AsSpan(
                DataFieldIndex.TransactionMessage.Response.PAN,
                DataFieldLength.PAN
            );

            // Find null terminator (0x00)
            int nullIndex = span.IndexOf((byte)0x00);

            // Slice up to the null or empty string
            var stringBytes = nullIndex > 0 ? span[..nullIndex] : Array.Empty<byte>();

            return Encoding.ASCII.GetString(stringBytes);
        }

    }

    /// <summary>
    /// Card holder name.
    /// </summary>
    public string PersonName 
    {
        get
        {
            var span = _message.AsSpan(
                DataFieldIndex.TransactionMessage.Response.PersonName,
                DataFieldLength.PersonName
            );

            // Find null terminator (0x00)
            int nullIndex = span.IndexOf((byte)0x00);

            // Slice up to the null or empty string
            var stringBytes = nullIndex > 0 ? span[..nullIndex] : span;

            return Encoding.ASCII.GetString(stringBytes);
        }

    }

    /// <summary>
    /// For loyalty transaction, this will be redemption amount.
    /// </summary>
    public decimal RedemptionAmount => Convert.ToDecimal(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.TransactionMessage.Response.RedemptionAmount, DataFieldLength.RedemptionAmount)
        )
    ) / 100m;

    public string RRN => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.RRN, DataFieldLength.RRN)
    );

    /// <summary>
    /// Net amount after rededcution amount deducted.
    /// </summary>
    public decimal NetAmount => Convert.ToDecimal(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.TransactionMessage.Response.NetAmount, DataFieldLength.NetAmount)
        )
    ) / 100m;

    /// <summary>
    /// True - signature required. False - signature NOT required.
    /// </summary>
    public bool SignatureNotRequiredIndicator => Convert.ToBoolean(
        _message[DataFieldIndex.TransactionMessage.Response.SignatureNotRequiredIndicator]
    );

    /// <summary>
    /// Terminal ID.
    /// </summary>
    public string TerminalId => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.TerminalId, DataFieldLength.TerminalId)
    );

    /// <summary>
    /// Terminal reference number assigned by the terminal.
    /// </summary>
    public string TerminalRefNo => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.TerminalRefNo, DataFieldLength.TerminalRefNo)
    );

    /// <summary>
    /// Additional data in TLV (Tag, Length, Value) format.
    /// Tag is 2 bytes characters, 3 bytes data length and the data.
    /// Will contain zero or more TLV fields.
    /// </summary>
    public string PrivateField => Encoding.ASCII.GetString(
        _message[DataFieldIndex.TransactionMessage.Response.PrivateField..^3]
    );

    /// <summary>
    /// Reponse code of the terminal.
    /// See Edc.Core.Common.ResponseCodes.cs for complete list.
    /// </summary>
    public override string ResponseCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.ResponseCode, DataFieldLength.ResponseCode)
    );
}