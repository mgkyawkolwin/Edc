using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Represents a transaction response message received from the host.
/// Contains transaction details such as approval, card info, amounts, and terminal info.
/// </summary>
public class TransactionResponseMessage : ResponseMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionResponseMessage"/> class.
    /// </summary>
    /// <param name="message">The raw byte array of the message.</param>
    public TransactionResponseMessage(byte[] message)
    {
        _message = message;
    }

    /// <summary>
    /// Approval code sent by the host (not terminal's code).
    /// </summary>
    public string ApprovalCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.ApprovalCode, DataFieldLength.ApprovalCode)
    );

    /// <summary>
    /// Transaction amount as sent in the request message.
    /// </summary>
    public decimal Amount => Convert.ToDecimal(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.TransactionMessage.Response.Amount, DataFieldLength.Amount)
        )
    ) / 100m;

    /// <summary>
    /// Batch number from the terminal. Daily transactions are batched together.
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
    /// Card label (e.g., VISA, MASTER, JCB).
    /// </summary>
    public string CardLabel
    {
        get
        {
            var span = _message.AsSpan(DataFieldIndex.TransactionMessage.Response.CardLabel, DataFieldLength.CardLabel);
            int nullIndex = span.IndexOf((byte)0x00);
            var stringBytes = nullIndex >= 0 ? span[..nullIndex] : span;
            return Encoding.ASCII.GetString(stringBytes);
        }
    }

    /// <summary>
    /// Card type (e.g., VI, MC, AM). See <see cref="Edc.Core.Common.CardTypes"/> for full list.
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
    /// Electronic Cash Register (ECR) reference number sent by the ECR.
    /// </summary>
    public string EcrRefNo => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.EcrRefNo, DataFieldLength.EcrRefNo)
    );

    /// <summary>
    /// Entry mode of the payment (e.g., 05 - Chip/Insert, 07 - Contactless, 02 - Mag-Stripe).
    /// See <see cref="Edc.Core.Common.EntryModes"/> for full list.
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
    /// Personal account number (PAN).
    /// </summary>
    public string PAN
    {
        get
        {
            var span = _message.AsSpan(DataFieldIndex.TransactionMessage.Response.PAN, DataFieldLength.PAN);
            int nullIndex = span.IndexOf((byte)0x00);
            var stringBytes = nullIndex > 0 ? span[..nullIndex] : Array.Empty<byte>();
            return Encoding.ASCII.GetString(stringBytes);
        }
    }

    /// <summary>
    /// Card holder's name.
    /// </summary>
    public string PersonName
    {
        get
        {
            var span = _message.AsSpan(DataFieldIndex.TransactionMessage.Response.PersonName, DataFieldLength.PersonName);
            int nullIndex = span.IndexOf((byte)0x00);
            var stringBytes = nullIndex > 0 ? span[..nullIndex] : span;
            return Encoding.ASCII.GetString(stringBytes);
        }
    }

    /// <summary>
    /// For loyalty transactions, this is the redemption amount.
    /// </summary>
    public decimal RedemptionAmount => Convert.ToDecimal(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.TransactionMessage.Response.RedemptionAmount, DataFieldLength.RedemptionAmount)
        )
    ) / 100m;

    /// <summary>
    /// Retrieval reference number.
    /// </summary>
    public string RRN => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.RRN, DataFieldLength.RRN)
    );

    /// <summary>
    /// Net amount after deduction of redemption amount.
    /// </summary>
    public decimal NetAmount => Convert.ToDecimal(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.TransactionMessage.Response.NetAmount, DataFieldLength.NetAmount)
        )
    ) / 100m;

    /// <summary>
    /// True if signature is NOT required, false if signature is required.
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
    /// Additional data in TLV (Tag-Length-Value) format. May contain zero or more TLV fields.
    /// </summary>
    public string PrivateField => Encoding.ASCII.GetString(
        _message[DataFieldIndex.TransactionMessage.Response.PrivateField..^3]
    );

    /// <summary>
    /// Response code of the terminal. See <see cref="Edc.Core.Common.ResponseCodes"/> for full list.
    /// </summary>
    public override string ResponseCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Response.ResponseCode, DataFieldLength.ResponseCode)
    );
}