using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Represents the response returned by the terminal for a
/// card inquiry before sale request.
/// </summary>
public class CardInquiryBeforeSaleResponseMessage : ResponseMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CardInquiryBeforeSaleResponseMessage"/> class
    /// using the raw response bytes received from the terminal.
    /// </summary>
    /// <param name="message">Raw ISO-8583 formatted message bytes.</param>
    public CardInquiryBeforeSaleResponseMessage(byte[] message)
    {
        _message = message;
    }

    /// <summary>
    /// Gets the amount supplied in the response message.
    /// </summary>
    public decimal Amount => Convert.ToDecimal(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.CardInquiryMessage.Response.Amount, DataFieldLength.Amount)
        )
    );

    /// <summary>
    /// Gets the ECR (POS) reference number echoed back from the terminal.
    /// </summary>
    public string EcrRefNo => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.CardInquiryMessage.Response.EcrRefNo, DataFieldLength.EcrRefNo)
    );

    /// <summary>
    /// Gets the private field data, containing extra TLV or proprietary terminal data.
    /// </summary>
    public byte[] PrivateField => _message[
        DataFieldIndex.CardInquiryMessage.Response.PrivateField..^3
    ];

    /// <summary>
    /// Gets the terminal response code indicating the outcome of the inquiry.
    /// </summary>
    public override string ResponseCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.CardInquiryMessage.Response.ResponseCode, DataFieldLength.ResponseCode)
    );
}