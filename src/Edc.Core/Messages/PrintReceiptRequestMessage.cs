using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Represents a request message to retrieve or print a report or receipt from the EDC terminal.
/// Can be used for reprint, settlement report, summary report, or detail report based on the <see cref="TransactionTypes"/> specified.
/// </summary>
public class PrintReceiptRequestMessage : RequestMessage
{
    private DateTime _postDateTime;
    private string _posID;
    private string _hostNumber;

    /// <summary>
    /// Initializes a new instance of the <see cref="PrintReceiptRequestMessage"/> class.
    /// </summary>
    /// <param name="transactionType">
    /// Type of transaction/report to request. For example:
    /// <see cref="TransactionTypes.REPRINT_RECEIPT"/> for receipt reprint.
    /// </param>
    /// <param name="posDateTime">Current POS DateTime. Default is <see cref="DateTime.MinValue"/>.</param>
    /// <param name="hostNumber">
    /// Host number to print from. "0" means all hosts; otherwise, specify host number of batch to print.
    /// </param>
    /// <param name="blockNumber">
    /// For the first package, send "000000". For subsequent packages, increment sequentially
    /// until the last block as sent by terminal.
    /// </param>
    /// <param name="invoiceTraceNo">
    /// For printing settlement/summary/detail reports, fill with zeroes.
    /// For reprint receipt, fill with invoice/trace number of receipt to be reprinted,
    /// or "000000" for the last transaction receipt.
    /// </param>
    /// <param name="posID">POS identifier. If unavailable, fill with spaces.</param>
    public PrintReceiptRequestMessage(
        TransactionTypes transactionType = TransactionTypes.REPRINT_RECEIPT,
        DateTime posDateTime = new DateTime(),
        string hostNumber = "0",
        string blockNumber = "0",
        string invoiceTraceNo = "0",
        string posID = "")
    {
        _postDateTime = posDateTime;
        _posID = posID;
        _hostNumber = hostNumber;

        // Build the data field
        byte[] _data = new byte[] {
            (byte)SenderIndicator,
            (byte) transactionType,
        }
        .Concat(Encoding.ASCII.GetBytes(MessageVersion))
        .Concat(Encoding.ASCII.GetBytes(DateTime.Now.ToString(Constants.DATETIME_FORMAT)))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetSpacePaddedPosID(_posID)))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetZeroPaddedHostNumber(_hostNumber)))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetZeroPaddedBlockNo(blockNumber)))
        .Concat(Encoding.ASCII.GetBytes(Constants.EMPTY_RECEIPT_TRACE_RESERVED_FIELD))
        .ToArray();

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
    /// Gets the POS DateTime field from the message.
    /// </summary>
    public string PosDateTime => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Request.PosDateTime, DataFieldLength.PosDateTime)
    );

    /// <summary>
    /// Gets the POS ID field from the message.
    /// </summary>
    public string PosID => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Request.PosID, DataFieldLength.PosID)
    );

    /// <summary>
    /// Gets the host number field from the message.
    /// </summary>
    public string HostNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Request.HostNumber, DataFieldLength.HostNumber)
    );

    /// <summary>
    /// Gets the block number field from the message.
    /// </summary>
    public string BlockNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Request.BlockNumber, DataFieldLength.BlockNumber)
    );

    /// <summary>
    /// Gets the invoice/trace number field from the message.
    /// </summary>
    public string InvoiceTraceNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Request.InvoiceTraceNumber, DataFieldLength.InvoiceTraceNumber)
    );
}