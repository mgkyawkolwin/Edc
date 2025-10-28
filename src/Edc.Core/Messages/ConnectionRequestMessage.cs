using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class ConnectionRequestMessage : RequestMessage
{
    public ConnectionRequestMessage()
    {
        // Build the data field
        byte[] _data = new byte[] {
            (byte)SenderIndicator,
            (byte) TransactionTypes.CONNECTION_TEST,
        }
        .Concat(Encoding.ASCII.GetBytes(MessageVersion))
        .Concat(Encoding.ASCII.GetBytes(DateTime.Now.ToString("yyyyMMddHHmmss")))
        .Concat(Encoding.ASCII.GetBytes(Constants.EMPTY_POS_ID))
        .Concat(Encoding.ASCII.GetBytes(Constants.EMPTY_RESERVED_FIELD)).ToArray();

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

    public string PosDateTime => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.ConnectionMessage.Response.PosDateTime , DataFieldLength.PosDateTime)
    );

    public string PosID => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.ConnectionMessage.Response.PosID , DataFieldLength.PosID)
    );

}