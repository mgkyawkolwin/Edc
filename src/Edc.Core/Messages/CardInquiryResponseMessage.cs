using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Represents a response message for a Card Inquiry operation.
/// Parses raw byte[] message data into structured fields.
/// </summary>
public class CardInquiryResponseMessage : ResponseMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CardInquiryResponseMessage"/> class.
    /// </summary>
    /// <param name="message">The raw byte array received from the terminal.</param>
    public CardInquiryResponseMessage(byte[] message)
    {
        _message = message;
    }

    /// <summary>
    /// Gets the ECR reference number from the message.
    /// </summary>
    public string EcrRefNo => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.CardInquiryMessage.Response.EcrRefNo, DataFieldLength.EcrRefNo)
    );

    /// <summary>
    /// Gets the private field data from the message (excluding the last 3 bytes).
    /// </summary>
    public byte[] PrivateField => _message[
        DataFieldIndex.CardInquiryMessage.Response.PrivateField..^3
    ];

    /// <summary>
    /// Gets the response code indicating the result of the transaction.
    /// </summary>
    public override string ResponseCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.CardInquiryMessage.Response.ResponseCode, DataFieldLength.ResponseCode)
    );
}