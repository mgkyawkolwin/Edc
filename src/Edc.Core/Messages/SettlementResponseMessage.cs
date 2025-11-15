using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Represents a settlement response message received from the EDC terminal.
/// Contains settlement results for one or multiple hosts, including totals and individual host summaries.
/// </summary>
public class SettlementResponseMessage : ResponseMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SettlementResponseMessage"/> class
    /// from the raw byte array message received from the terminal.
    /// </summary>
    /// <param name="message">The raw message bytes from the terminal.</param>
    public SettlementResponseMessage(byte[] message)
    {
        _message = message;
    }

    /// <summary>
    /// Gets the POS datetime from the message.
    /// </summary>
    public string PosDateTime => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Response.PosDateTime, DataFieldLength.PosDateTime)
    );

    /// <summary>
    /// Gets the POS ID from the message.
    /// </summary>
    public string PosID => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Response.PosID, DataFieldLength.PosID)
    );

    /// <summary>
    /// Gets the host number included in the settlement message.
    /// </summary>
    public string HostNumber => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Response.HostNumber, DataFieldLength.HostNumber)
    );

    /// <summary>
    /// Gets the total number of hosts included in the settlement summary.
    /// </summary>
    public int HostCount => Convert.ToInt32(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.HostCount, DataFieldLength.HostCount)
        )
    );

    /// <summary>
    /// Gets the specific host number of the settlement summary record.
    /// </summary>
    public string HostNumberSettlement => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.HostNumber, DataFieldLength.HostNumberSettlement)
    );

    /// <summary>
    /// Gets the host name included in the settlement summary.
    /// </summary>
    public string HostName => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.HostName, DataFieldLength.HostName)
    );

    /// <summary>
    /// Gets the settlement result for the host (e.g., success or failure code).
    /// </summary>
    public string SettlementResult => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.SettlementResult, DataFieldLength.SettlementResult)
    );

    /// <summary>
    /// Gets the total number of sale transactions in the settlement.
    /// </summary>
    public int TotalSaleCount => Convert.ToInt32(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.TotalSaleCount, DataFieldLength.TotalSaleCount)
        )
    );

    /// <summary>
    /// Gets the total sale amount in the settlement, in currency units (divided by 100 from minor units).
    /// </summary>
    public decimal TotalSaleAmount => Convert.ToInt32(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.TotalSaleAmount, DataFieldLength.TotalSaleAmount)
        )
    ) / 100m;

    /// <summary>
    /// Gets the total number of refund transactions in the settlement.
    /// </summary>
    public int TotalRefuncCount => Convert.ToInt32(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.TotalRefundCount, DataFieldLength.TotalRefundCount)
        )
    );

    /// <summary>
    /// Gets the total refund amount in the settlement, in currency units (divided by 100 from minor units).
    /// </summary>
    public decimal TotalRefundAmount => Convert.ToInt32(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.SettlementMessage.Response.SettlementSummary.TotalRefundAmount, DataFieldLength.TotalRefundAmount)
        )
    ) / 100m;

    /// <summary>
    /// Gets the response code for the settlement operation.
    /// </summary>
    /// <remarks>
    /// - "00" indicates settlement successful.
    /// - "NU" indicates all batches are empty, no settlement performed.
    /// - "ER" indicates one or more host settlements failed. 
    ///   For multiple hosts settlement, the terminal will indicate failure if one or more host failed.
    ///   Refer to the Settlement Summary for further details.
    /// </remarks>
    public override string ResponseCode => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.SettlementMessage.Response.ResponseCode, DataFieldLength.ResponseCode)
    );
}