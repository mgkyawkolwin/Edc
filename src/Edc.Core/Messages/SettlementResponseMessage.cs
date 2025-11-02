using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class SettlementResponseMessage : ResponseMessage
{
    public SettlementResponseMessage(byte[] message)
    {
        _message = message;
    }

    public string PosDateTime => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Response.PosDateTime, DataFieldLength.PosDateTime)
    );

    public string PosID => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Response.PosID, DataFieldLength.PosID)
    );

    public string HostNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Response.HostNumber, DataFieldLength.HostNumber)
    );

    public int HostCount => Convert.ToInt32(
        Encoding.ASCII.GetString(
                _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.HostCount, DataFieldLength.HostCount)
        )
    );

    public string HostNumberSettlement => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.HostNumber, DataFieldLength.HostNumberSettlement)
    );

    public string HostName => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.HostName, DataFieldLength.HostName)
    );

    public string SettlementResult => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.SettlementResult, DataFieldLength.SettlementResult)
    );

    public int TotalSaleCount => Convert.ToInt32(
        Encoding.ASCII.GetString(
                _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.TotalSaleCount, DataFieldLength.TotalSaleCount)
        )
    );

    public decimal TotalSaleAmount => Convert.ToInt32(
        Encoding.ASCII.GetString(
                _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.TotalSaleAmount, DataFieldLength.TotalSaleAmount)
        )
    ) / 100m;

    public int TotalRefuncCount => Convert.ToInt32(
        Encoding.ASCII.GetString(
                _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.TotalRefundCount, DataFieldLength.TotalRefundCount)
        )
    );

    public decimal TotalRefundAmount => Convert.ToInt32(
        Encoding.ASCII.GetString(
                _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.TotalRefundAmount, DataFieldLength.TotalRefundAmount)
        )
    ) / 100m;



    /// <summary>
    /// 00 - Settlement successful
    /// NU - All batches are empty. No settlement is performed.
    /// ER - One or more host settlement failed.
    /// </summary>
    /// <remarks>
    /// For multiple hosts settlement (settle ALL) terminal
    /// will indicate failure if settlement of one or more host
    /// failed. Refer to Settlement Summary for further information.
    /// </remarks>
    public override string ResponseCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Response.ResponseCode, DataFieldLength.ResponseCode)
    );
}