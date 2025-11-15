namespace Edc.Core.Common;

/// <summary>
/// Defines constants for supported card and payment types.
/// These codes are typically used in ISO-8583 messages or internal transaction processing.
/// </summary>
public static class CardTypes
{
    /// <summary>Visa card.</summary>
    public const string VISA = "VI";

    /// <summary>MasterCard.</summary>
    public const string MASTERCARD = "MC";

    /// <summary>American Express card.</summary>
    public const string AMEX = "AM";

    /// <summary>Diners Club card.</summary>
    public const string DINERS_CLUB = "DI";

    /// <summary>JCB card.</summary>
    public const string JCB = "JC";

    /// <summary>China UnionPay card.</summary>
    public const string CHINA_UNION_PAY = "DB";

    /// <summary>Alipay payment.</summary>
    public const string ALIPAY = "AP";

    /// <summary>WeChat Pay payment.</summary>
    public const string WECHAT_PAY = "WP";

    /// <summary>UPI QR code payment.</summary>
    public const string UPI_QR = "UP";

    /// <summary>PayNow payment.</summary>
    public const string PAYNOW = "PN";

    /// <summary>Link Card payment.</summary>
    public const string LINK_CARD = "LN";

    /// <summary>UPlus Card payment.</summary>
    public const string UPLUS_CARD = "UP";
}