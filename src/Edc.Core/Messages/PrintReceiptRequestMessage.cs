using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Request message to retrieve print/report message.
/// </summary>
public class PrintReceiptRequestMessage : RequestMessage
{
    private DateTime _postDateTime;
    private string _posID;
    private string _hostNumber;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="posDateTime">Current DateTime of pos application.</param>
    /// <param name="hostNumber">0 - all host. N - host number of the batch to be printed.</param>
    /// <param name="blockNumber">For the first package, send 000000. Subsequently 000001, 000002, until last block as sent by terminal.</param>
    /// <param name="invoiceTraceNo">For printing of settlement/summary/detail report fills Number in with zeroes. For reprint receipt, fill in with Invoice/Trace Number of receipt to be reprinted, or 000000 for last transaction receipt.</param>
    /// <param name="posID">Put POS ID here. If not available just fill in with spaces.</param>
    public PrintReceiptRequestMessage(TransactionTypes transactionType = TransactionTypes.REPRINT_RECEIPT, DateTime posDateTime = new DateTime(), string hostNumber = "0", string blockNumber = "0", string invoiceTraceNo = "0", string posID = "")
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

    public string PosDateTime => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Request.PosDateTime, DataFieldLength.PosDateTime)
    );

    public string PosID => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Request.PosID, DataFieldLength.PosID)
    );

    public string HostNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Request.HostNumber, DataFieldLength.HostNumber)
    );

    public string BlockNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Request.BlockNumber, DataFieldLength.BlockNumber)
    );

    public string InvoiceTraceNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Request.InvoiceTraceNumber, DataFieldLength.InvoiceTraceNumber)
    );

}