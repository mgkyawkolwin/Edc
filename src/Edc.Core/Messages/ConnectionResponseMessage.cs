using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Represents a response message for a POS–Host connection test operation.
/// </summary>
/// <remarks>
/// The message contains the host's returned POS date/time, the POS ID,
/// and a response code indicating whether the connection test succeeded.
/// </remarks>
public class ConnectionResponseMessage : ResponseMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionResponseMessage"/> class
    /// using the raw message bytes received from the host.
    /// </summary>
    /// <param name="message">The full response message byte array.</param>
    public ConnectionResponseMessage(byte[] message)
    {
        _message = message;
    }

    /// <summary>
    /// Gets the POS date and time returned by the host.
    /// </summary>
    public string PosDateTime => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.ConnectionMessage.Response.PosDateTime, DataFieldLength.PosDateTime)
    );

    /// <summary>
    /// Gets the POS ID returned by the host.
    /// </summary>
    public string PosID => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.ConnectionMessage.Response.PosID, DataFieldLength.PosID)
    );

    /// <summary>
    /// Gets the response code.
    /// <para>"00" indicates a successful connection.</para>
    /// </summary>
    public override string ResponseCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.ConnectionMessage.Response.ResponseCode, DataFieldLength.ResponseCode)
    );
}