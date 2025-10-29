using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class SettlementRequestMessage : RequestMessage
{
    private DateTime _postDateTime;
    private string _posID;
    private int _hostNumber;

    public SettlementRequestMessage(DateTime postDateTime, int hostNumber, string posID = Constants.EMPTY_POS_ID)
    {
        _postDateTime = postDateTime;
        _posID = posID;
        _hostNumber = hostNumber;

        // Build the data field
        byte[] _data = new byte[] {
            (byte)SenderIndicator,
            (byte) TransactionTypes.SETTLEMENT,
        }
        .Concat(Encoding.ASCII.GetBytes(MessageVersion))
        .Concat(Encoding.ASCII.GetBytes(DateTime.Now.ToString("yyyyMMddHHmmss")))
        .Concat(Encoding.ASCII.GetBytes(_posID))
        .Concat(Encoding.ASCII.GetBytes(Convert.ToString(_hostNumber))).ToArray();

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