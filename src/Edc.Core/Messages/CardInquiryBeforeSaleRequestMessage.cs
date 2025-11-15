using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Represents a CARD_ENQUIRY_BEFORE_SALES request message sent to the EDC terminal.
/// This is typically used to retrieve card information (PAN, card type, etc.)
/// before performing an actual sale transaction.
/// </summary>
/// <remarks>
/// The message structure follows the ISO-8583-like format defined by EDC:
/// - STX 
/// - BCD-encoded data length
/// - Message data fields (sender, transaction type, version, ECR ref, amount, terminal ref)
/// - ETX
/// - LRC checksum
/// </remarks>
public class CardInquiryBeforeSaleRequestMessage : RequestMessage
{
    /// <summary>
    /// Creates a new instance of the <see cref="CardInquiryBeforeSaleRequestMessage"/>.
    /// Builds and encodes the complete request packet (STX, fields, ETX, LRC).
    /// </summary>
    /// <param name="ecrRefNo">
    /// The ECR reference number. Will be space-padded automatically to match the protocol’s
    /// required format.
    /// </param>
    /// <param name="amount">
    /// The transaction amount. Encoded as a zero-padded 12–digit string.
    /// </param>
    /// <param name="terminalRefNo">
    /// Terminal reference number (optional). Defaults to empty (space padded).
    /// </param>
    public CardInquiryBeforeSaleRequestMessage(
        string ecrRefNo,
        decimal amount,
        string terminalRefNo = Constants.EMPTY_TERMINAL_REF_NO)
    {
        // Build the data field
        byte[] _data = new byte[] {
            (byte)SenderIndicator,
            (byte) TransactionTypes.CARD_ENQUIRY_BEFORE_SALES,
        }
        .Concat(Encoding.ASCII.GetBytes(MessageVersion))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetSpacePaddedEcrRefNo(ecrRefNo)))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetZeroPaddedAmount(amount)))
        .Concat(Encoding.ASCII.GetBytes(terminalRefNo))
        .ToArray();

        // Compute BCD encoded length
        byte[] bcd = BCDConverter.ToBCD(_data.Length);

        // Compute LRC checksum
        byte lrc = LRCCalculator.Calculate(
            Array.Empty<byte>()
            .Concat(bcd)
            .Concat(_data)
            .Concat(new byte[] { ETX })
            .ToArray()
        );

        // Construct full message payload
        _message = Array.Empty<byte>()
            .Concat(new byte[] { STX })
            .Concat(bcd)
            .Concat(_data)
            .Concat(new byte[] { ETX })
            .Concat(new byte[] { lrc })
            .ToArray();
    }

    /// <summary>
    /// Gets the transaction amount included in the request message.
    /// Extracted from the encoded message byte array.
    /// </summary>
    public decimal Amount => Convert.ToDecimal(
        Encoding.ASCII.GetString(
            _message.AsSpan(
                DataFieldIndex.CardInquiryMessage.Response.Amount,
                DataFieldLength.Amount
            )
        )
    );
}