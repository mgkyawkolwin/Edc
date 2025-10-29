using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class CardInquiryRequestMessage : RequestMessage
{
    private decimal _amount = 0.0M;
    private string _ecrRefNo;
    private string _terminalRefNo;

    public CardInquiryRequestMessage(string ecrRefNo, string terminalRefNo = Constants.EMPTY_TERMINAL_REF_NO)
    {
        _ecrRefNo = ecrRefNo;
        _terminalRefNo = terminalRefNo;
        // Build the data field
        byte[] _data = new byte[] {
            (byte)SenderIndicator,
            (byte) TransactionTypes.CARD_ENQUIRY,
        }
        .Concat(Encoding.ASCII.GetBytes(MessageVersion))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetPaddedEcrRefNo(_ecrRefNo)))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetPaddedAmount(_amount)))
        .Concat(Encoding.ASCII.GetBytes(_terminalRefNo)).ToArray();

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