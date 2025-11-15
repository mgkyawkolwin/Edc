using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Represents the request message used to perform a card inquiry operation.
/// This inquiry checks card-related information before performing any sale or transaction.
/// </summary>
public class CardInquiryRequestMessage : RequestMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CardInquiryRequestMessage"/> class.
    /// Builds the raw ISO-8583-like message including STX, BCD length, data fields, ETX, and LRC.
    /// </summary>
    /// <param name="ecrRefNo">
    /// The ECR reference number assigned by the POS.  
    /// This field is padded with spaces if shorter than required.
    /// </param>
    /// <param name="terminalRefNo">
    /// The terminal reference number.  
    /// Defaults to <see cref="Constants.EMPTY_TERMINAL_REF_NO"/>.
    /// </param>
    public CardInquiryRequestMessage(string ecrRefNo, string terminalRefNo = Constants.EMPTY_TERMINAL_REF_NO)
    {
        // Build the data field
        byte[] _data = new byte[] {
            (byte)SenderIndicator,
            (byte) TransactionTypes.CARD_ENQUIRY,
        }
        .Concat(Encoding.ASCII.GetBytes(MessageVersion))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetSpacePaddedEcrRefNo(ecrRefNo)))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetZeroPaddedAmount(0)))
        .Concat(Encoding.ASCII.GetBytes(terminalRefNo))
        .ToArray();

        // Compute BCD
        byte[] bcd = BCDConverter.ToBCD(_data.Length);

        // Calculate LRC
        byte lrc = LRCCalculator.Calculate(
            Array.Empty<byte>().Concat(bcd).Concat(_data).Concat(new byte[] { ETX }).ToArray()
        );

        // Build the complete message
        _message = Array.Empty<byte>()
            .Concat(new byte[] { STX })
            .Concat(bcd)
            .Concat(_data)
            .Concat(new byte[] { ETX })
            .Concat(new byte[] { lrc })
            .ToArray();
    }
}