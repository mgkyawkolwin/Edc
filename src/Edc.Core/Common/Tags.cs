namespace Edc.Core.Common;

/// <summary>
/// Defines standard tags used in EDC messages for transaction and card inquiry responses.
/// These tags help identify specific fields in message payloads.
/// </summary>
public static class Tags
{
    // Transaction Response Tags

    /// <summary>Private field (custom or reserved data).</summary>
    public const string PrivateField = "PF";

    /// <summary>Application label (e.g., card application name).</summary>
    public const string ApplicationLabel = "AL";

    /// <summary>Application identifier (AID).</summary>
    public const string ApplicationId = "AI";

    /// <summary>Transaction cryptogram (TC).</summary>
    public const string TC = "TC";

    /// <summary>Trace number of the transaction.</summary>
    public const string TraceNumber = "TN";

    /// <summary>Response text returned from the host.</summary>
    public const string ResponseText = "RT";

    /// <summary>Terminal verification result.</summary>
    public const string TerminalVerificationResult = "TR";

    /// <summary>Loyalty points earned in the transaction.</summary>
    public const string LoyaltyEarned = "LE";

    /// <summary>Loyalty number associated with the cardholder.</summary>
    public const string LoyaltyNumber = "LN";

    /// <summary>Loyalty statement details.</summary>
    public const string LoyaltyStatement = "LS";

    /// <summary>Discount amount applied to the transaction.</summary>
    public const string DiscountAmount = "DA";

    /// <summary>Balance amount of the account or card.</summary>
    public const string BalanceAmount = "BL";

    /// <summary>Expiry date of the balance.</summary>
    public const string BalanceExpiryDate = "BE";

    /// <summary>Terminal-side transaction identifier.</summary>
    public const string TerminalTransactionId = "Q1";

    /// <summary>Host-side transaction identifier.</summary>
    public const string HostTransactionId = "Q2";

    /// <summary>Login ID associated with the terminal or user.</summary>
    public const string LoginId = "Q3";

    /// <summary>Exchange rate to SGD.</summary>
    public const string ExchangeRateSGD = "Q4";

    /// <summary>Total amount in RMB currency.</summary>
    public const string TotalAmountRMB = "Q5";

    // Card Inquiry Response Tags

    /// <summary>Hash of the card number.</summary>
    public const string CardNumberHash = "CH";

    /// <summary>Masked card number for display purposes.</summary>
    public const string CardNumberMasked = "CM";

    /// <summary>Card number including PAN.</summary>
    public const string CardNumberWithPAN = "CN";

    /// <summary>Card type code.</summary>
    public const string CardType = "CT";

    /// <summary>Entry mode used for the transaction (chip, swipe, manual, etc.).</summary>
    public const string EntryMode = "EM";

    /// <summary>Integrated circuit number (IC number) of the card.</summary>
    public const string ICNumber = "IC";

    /// <summary>Payer name associated with the card.</summary>
    public const string PayerName = "PN";

    /// <summary>Standard length of a tag value in bytes.</summary>
    public const int TagValueLength = 3; // 3 bytes
}