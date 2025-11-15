using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Represents a settlement request message sent from the POS to the EDC terminal.
/// Used to trigger the settlement process for all or specific hosts.
/// </summary>
public class SettlementRequestMessage : RequestMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SettlementRequestMessage"/> class.
    /// Builds the complete message including STX, BCD length, data fields, ETX, and LRC.
    /// </summary>
    /// <param name="postDateTime">
    /// Current datetime of the POS machine or a reference datetime.
    /// If not specified, defaults to <see cref="DateTime.MinValue"/>.
    /// </param>
    /// <param name="hostNumber">
    /// The host number to settle. "000" to settle all hosts, or a specific 3-digit host number.
    /// </param>
    /// <param name="posID">
    /// POS ID string (maximum 6 characters). If not specified, can be empty or space-padded.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="hostNumber"/> is longer than 3 characters or <paramref name="posID"/> is longer than 6 characters.
    /// </exception>
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

    /// <summary>
    /// Gets the POS DateTime included in the message.
    /// </summary>
    public DateTime PosDateTime => Helper.GetDateTime(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.SettlementMessage.Request.PosDateTime, DataFieldLength.PosDateTime)
        )
    );

    /// <summary>
    /// Gets the POS ID included in the message.
    /// </summary>
    public string PosID => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Request.PosID, DataFieldLength.PosID)
    ).Trim();

    /// <summary>
    /// Gets the host number included in the message.
    /// </summary>
    public string HostNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Request.HostNumber, DataFieldLength.HostNumber)
    ).Trim();
}