using Edc.Core.Common;
using Edc.Core.Messages;

namespace Edc.Core.Factories;

/// <summary>
/// Factory class responsible for creating response messages based on transaction types.
/// </summary>
public static class ResponseMessageFactory
{
    /// <summary>
    /// Creates a response message corresponding to the transaction type in the provided response data.
    /// </summary>
    /// <param name="data">The response data received from the EDC terminal.</param>
    /// <returns>
    /// A <see cref="ResponseMessage"/> instance. 
    /// The client should cast it to the specific response message type as needed.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown when the transaction type in the data is not supported.
    /// </exception>
    /// <remarks>
    /// The mapping of transaction types to response message classes is as follows:
    /// <list type="bullet">
    ///   <item>
    ///     <description><see cref="TransactionResponseMessage"/>: 
    ///     <see cref="TransactionTypes.SALE_FULL_PAYMENT"/>, 
    ///     <see cref="TransactionTypes.VOID"/>, 
    ///     <see cref="TransactionTypes.REFUND"/>, 
    ///     <see cref="TransactionTypes.TIP_ADJUST"/></description>
    ///   </item>
    ///   <item>
    ///     <description><see cref="ConnectionResponseMessage"/>: <see cref="TransactionTypes.CONNECTION_TEST"/></description>
    ///   </item>
    ///   <item>
    ///     <description><see cref="CardInquiryResponseMessage"/>: <see cref="TransactionTypes.CARD_ENQUIRY"/></description>
    ///   </item>
    ///   <item>
    ///     <description><see cref="CardInquiryBeforeSaleResponseMessage"/>: <see cref="TransactionTypes.CARD_ENQUIRY_BEFORE_SALES"/></description>
    ///   </item>
    ///   <item>
    ///     <description><see cref="PrintReceiptResponseMessage"/>: 
    ///     <see cref="TransactionTypes.PRINT_SETTLEMENT_REPORT"/>, 
    ///     <see cref="TransactionTypes.PRINT_SUMMARY_REPORT"/>, 
    ///     <see cref="TransactionTypes.PRINT_DETAIL_REPORT"/>, 
    ///     <see cref="TransactionTypes.REPRINT_RECEIPT"/></description>
    ///   </item>
    ///   <item>
    ///     <description><see cref="TransactionStatusUpdateResponseMessage"/>: <see cref="TransactionTypes.STATUS_UPDATE"/></description>
    ///   </item>
    ///   <item>
    ///     <description><see cref="SettlementResponseMessage"/>: <see cref="TransactionTypes.SETTLEMENT"/></description>
    ///   </item>
    /// </list>
    /// </remarks>
    public static ResponseMessage CreateResponseMessage(byte[] data)
    {
        return (TransactionTypes)data[DataFieldIndex.TransactionType] switch
        {
            TransactionTypes.SALE_FULL_PAYMENT or TransactionTypes.VOID or TransactionTypes.REFUND or TransactionTypes.TIP_ADJUST => new TransactionResponseMessage(data),
            TransactionTypes.CONNECTION_TEST => new ConnectionResponseMessage(data),
            TransactionTypes.CARD_ENQUIRY => new CardInquiryResponseMessage(data),
            TransactionTypes.CARD_ENQUIRY_BEFORE_SALES => new CardInquiryBeforeSaleResponseMessage(data),
            TransactionTypes.PRINT_SETTLEMENT_REPORT or TransactionTypes.PRINT_SUMMARY_REPORT or TransactionTypes.PRINT_DETAIL_REPORT or TransactionTypes.REPRINT_RECEIPT => new PrintReceiptResponseMessage(data),
            TransactionTypes.STATUS_UPDATE => new TransactionStatusUpdateResponseMessage(data),
            TransactionTypes.SETTLEMENT => new SettlementResponseMessage(data),
            _ => throw new NotSupportedException($"Unsupported transaction type: {data[4]:X2}"),
        };
    }
}