using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Represents a connection test request message sent from POS to EDC.
/// </summary>
public class ConnectionRequestMessage : RequestMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionRequestMessage"/> class.
    /// Builds the full request message including STX, length (BCD), data fields, ETX, and LRC.
    /// </summary>
    /// <param name="dateTime">
    /// The POS date and time to include in the request message.
    /// If not provided, an empty <see cref="DateTime"/> is used.
    /// </param>
    /// <param name="posId">
    /// The POS terminal ID. It will be padded to the required length.
    /// </param>
    public ConnectionRequestMessage(DateTime dateTime = new DateTime(), string posId = "")
    {
        // Build the data field
        byte[] _data = new byte[] {
            (byte)SenderIndicator,
            (byte)TransactionTypes.CONNECTION_TEST,
        }
        .Concat(Encoding.ASCII.GetBytes(MessageVersion))
        .Concat(Encoding.ASCII.GetBytes(dateTime.ToString(Constants.DATETIME_FORMAT)))
        .Concat(Encoding.ASCII.GetBytes(posId.PadLeft(DataFieldLength.PosID, Constants.SPACE_CHAR)))
        .Concat(Encoding.ASCII.GetBytes(Constants.EMPTY_RESERVED_FIELD))
        .ToArray();

        // Compute BCD
        byte[] bcd = BCDConverter.ToBCD(_data.Length);

        // Calculate LRC
        byte lrc = LRCCalculator.Calculate(
            Array.Empty<byte>()
                .Concat(bcd)
                .Concat(_data)
                .Concat(new byte[] { ETX })
                .ToArray()
        );

        // Build complete message
        _message = Array.Empty<byte>()
            .Concat(new byte[] { STX })
            .Concat(bcd)
            .Concat(_data)
            .Concat(new byte[] { ETX })
            .Concat(new byte[] { lrc })
            .ToArray();
    }

    /// <summary>
    /// Gets the POS datetime extracted from the response message.
    /// </summary>
    public string PosDateTime => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.ConnectionMessage.Response.PosDateTime, DataFieldLength.PosDateTime)
    );

    /// <summary>
    /// Gets the POS terminal ID extracted from the response message.
    /// </summary>
    public string PosID => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.ConnectionMessage.Response.PosID, DataFieldLength.PosID)
    );
}