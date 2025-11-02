using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Represents a settlement request message
/// </summary>
public class SettlementRequestMessage : RequestMessage
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="postDateTime">Current datetime of pos machine or could be any reference value.</param>
    /// <param name="hostNumber">000 - settle all hosts. NNN - host number of the batch to be printed.</param>
    /// <param name="posID">Pos ID or empty space.</param>
    public SettlementRequestMessage(DateTime postDateTime = new DateTime(), string hostNumber = "0", string posID = "")
    {
        if (hostNumber.Length > 3) throw new ArgumentOutOfRangeException(nameof(hostNumber), "The length of hostNumner should be <= 3.");
        if (posID.Length > 6) throw new ArgumentOutOfRangeException(nameof(posID), "The length of PosID should be <= 6.");

        // Build the data field
        byte[] _data = new byte[] {
            (byte)SenderIndicator,
            (byte) TransactionTypes.SETTLEMENT,
        }
        .Concat(Encoding.ASCII.GetBytes(MessageVersion))
        .Concat(Encoding.ASCII.GetBytes(DateTime.Now.ToString(Constants.DATETIME_FORMAT)))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetSpacePaddedPosID(posID)))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetZeroPaddedHostNumber(hostNumber))).ToArray();

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

    public DateTime PosDateTime => Helper.GetDateTime(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.SettlementMessage.Request.PosDateTime, DataFieldLength.PosDateTime)
        )
    );

    public string PosID => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Request.PosID, DataFieldLength.PosID)
    ).Trim();

    public string HostNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Request.HostNumber, DataFieldLength.HostNumber)
    ).Trim();

}