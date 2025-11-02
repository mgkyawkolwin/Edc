using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class PrintReceiptResponseMessage : ResponseMessage
{
    public PrintReceiptResponseMessage(byte[] message)
    {
        _message = message;
    }

    public string PosDateTime => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Response.PosDateTime , DataFieldLength.PosDateTime)
    );

    public string PosID => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Response.PosID , DataFieldLength.PosID)
    );

    public string HostNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Response.HostNumber, DataFieldLength.HostNumber)
    );

    public string BlockNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Response.PosID , DataFieldLength.BlockNumber)
    );

    public string PrintingData => Encoding.ASCII.GetString(
        _message[DataFieldIndex.PrintReceiptMessage.Response.PrintingData .. ^3]
    );

    public int PrintingDataLength => Convert.ToInt32(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Response.PrintingDataLength, DataFieldLength.PrintingDataLength)
        )
    );

    /// <summary>
    /// "00" - No error, data available for printing.
    /// "NF" = Not Found, No data available for printing.
    /// </summary>
    public override string ResponseCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.PrintReceiptMessage.Response.ResponseCode, DataFieldLength.ResponseCode)
    );
}