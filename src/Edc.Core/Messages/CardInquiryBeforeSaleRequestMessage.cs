using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class CardInquiryBeforeSaleRequestMessage : RequestMessage
{

    public CardInquiryBeforeSaleRequestMessage(string ecrRefNo, decimal amount, string terminalRefNo = Constants.EMPTY_TERMINAL_REF_NO)
    {
        // Build the data field
        byte[] _data = new byte[] {
            (byte)SenderIndicator,
            (byte) TransactionTypes.CARD_ENQUIRY_BEFORE_SALES,
        }
        .Concat(Encoding.ASCII.GetBytes(MessageVersion))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetZeroPaddedEcrRefNo(ecrRefNo)))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetZeroPaddedAmount(amount)))
        .Concat(Encoding.ASCII.GetBytes(terminalRefNo)).ToArray();

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

    public decimal Amount => Convert.ToDecimal(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.CardInquiryMessage.Response.Amount, DataFieldLength.Amount)
        )
    );

}