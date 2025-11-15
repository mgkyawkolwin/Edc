namespace Edc.Core.Common;

/// <summary>
/// Represents the different transaction types supported by the EDC terminal.
/// Each transaction type is associated with a specific byte code used in ISO-8583 or proprietary messages.
/// </summary>
public enum TransactionTypes : byte
{
    /// <summary>Full payment sale transaction.</summary>
    SALE_FULL_PAYMENT = 0x30,

    /// <summary>Loyalty redemption sale transaction.</summary>
    SALE_LOYALTY_REDEMPTION = 0x3A,

    /// <summary>Instalment payment transaction.</summary>
    INSTALMENT = 0x31,

    /// <summary>Offline sale transaction.</summary>
    OFFLINE_SALE = 0x32,

    /// <summary>PayWave contactless sale transaction.</summary>
    PAYWAVE_SALE = 0x33,

    /// <summary>Void a previously completed transaction.</summary>
    VOID = 0x34,

    /// <summary>Refund a previously completed transaction.</summary>
    REFUND = 0x35,

    /// <summary>Card verification or pre-authorization transaction.</summary>
    CARD_VERIFY_PREAUTH = 0x36,

    /// <summary>Tip adjustment for a previous transaction.</summary>
    TIP_ADJUST = 0x37,

    /// <summary>China UnionPay sale transaction.</summary>
    CUP_SALE = 0x40,

    /// <summary>China UnionPay void transaction.</summary>
    CUP_VOID = 0x44,

    /// <summary>China UnionPay refund transaction.</summary>
    CUP_REFUND = 0x45,

    /// <summary>Alipay sale transaction.</summary>
    ALIPAY_SALE = 0x41,

    /// <summary>WeChat Pay sale transaction.</summary>
    WECHAT_SALE = 0x42,

    /// <summary>UPI QR sale transaction.</summary>
    UPI_QR_SALE = 0x43,

    /// <summary>PayNow sale transaction.</summary>
    PAYNOW_SALE = 0x4E,

    /// <summary>Card inquiry transaction.</summary>
    CARD_ENQUIRY = 0x50,

    /// <summary>Card inquiry before sale transaction.</summary>
    CARD_ENQUIRY_BEFORE_SALES = 0x51,

    /// <summary>Print settlement report.</summary>
    PRINT_SETTLEMENT_REPORT = 0x52,

    /// <summary>Settlement transaction.</summary>
    SETTLEMENT = 0x53,

    /// <summary>Print summary report.</summary>
    PRINT_SUMMARY_REPORT = 0x54,

    /// <summary>Print detailed report.</summary>
    PRINT_DETAIL_REPORT = 0x55,

    /// <summary>Reprint a previously printed receipt.</summary>
    REPRINT_RECEIPT = 0x56,

    /// <summary>Status update message from the terminal.</summary>
    STATUS_UPDATE = 0x60,

    /// <summary>Connection test message to verify terminal connectivity.</summary>
    CONNECTION_TEST = 0x63,
}