using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Represents the response message for a print/receipt request from the EDC terminal.
/// Contains information about the report or receipt data, block numbers, and status.
/// </summary>
public class PrintReceiptResponseMessage : ResponseMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PrintReceiptResponseMessage"/> class with raw message bytes.
    /// </summary>
    /// <param name="message">Raw message bytes received from the terminal.</param>
    public PrintReceiptResponseMessage(byte[] message)
    {
        _message = message;
    }

    /// <summary>
    /// Gets the POS DateTime returned by the terminal.
    /// </summary>
    public string PosDateTime => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Response.PosDateTime, DataFieldLength.PosDateTime)
    );

    /// <summary>
    /// Gets the POS ID returned by the terminal.
    /// </summary>
    public string PosID => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Response.PosID, DataFieldLength.PosID)
    );

    /// <summary>
    /// Gets the host number of the report/receipt returned by the terminal.
    /// </summary>
    public string HostNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Response.HostNumber, DataFieldLength.HostNumber)
    );

    /// <summary>
    /// Gets the block number of the report/receipt returned by the terminal.
    /// </summary>
    public string BlockNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Response.PosID, DataFieldLength.BlockNumber)
    );

    /// <summary>
    /// Gets the actual printing data returned by the terminal.
    /// </summary>
    public string PrintingData => Encoding.ASCII.GetString(
        _message[DataFieldIndex.PrintReceiptMessage.Response.PrintingData..^3]
    );

    /// <summary>
    /// Gets the length of the printing data returned by the terminal.
    /// </summary>
    public int PrintingDataLength => Convert.ToInt32(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Response.PrintingDataLength, DataFieldLength.PrintingDataLength)
        )
    );

    /// <summary>
    /// Gets the response code of the message.
    /// "00" indicates no error, data available for printing.
    /// "NF" indicates not found, no data available for printing.
    /// </summary>
    public override string ResponseCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Response.ResponseCode, DataFieldLength.ResponseCode)
    );
}