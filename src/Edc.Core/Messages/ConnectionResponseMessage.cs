using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Represents a connection response message.
/// </summary>
public class ConnectionResponseMessage : ResponseMessage
{
    public ConnectionResponseMessage(byte[] message)
    {
        _message = message;
    }

    public string PosDateTime => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.ConnectionMessage.Response.PosDateTime, DataFieldLength.PosDateTime)
    );

    public string PosID => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.ConnectionMessage.Response.PosID, DataFieldLength.PosID)
    );

    /// <summary>
    /// "00" - OK
    /// </summary>
    public override string ResponseCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.ConnectionMessage.Response.ResponseCode, DataFieldLength.ResponseCode)
    );
}