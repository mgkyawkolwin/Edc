namespace Edc.Core.Common;

public static class Tags
{
    // Transaction Response Tags
    public const string PrivateField = "PF";
    public const string ApplicationLabel = "AL";
    public const string ApplicationId = "AI";
    public const string TC = "TC";
    public const string TraceNumber = "TN";
    public const string ResponseText = "RT";
    public const string TerminalVerificationResult = "TR";
    public const string LoyaltyEarned = "LE";
    public const string LoyaltyNumber = "LN";
    public const string LoyaltyStatement = "LS";
    public const string DiscountAmount = "DA";
    public const string BalanceAmount = "BL";
    public const string BalanceExpiryDate = "BE";
    public const string TerminalTransactionId = "Q1";
    public const string HostTransactionId = "Q2";
    public const string LoginId = "Q3";
    public const string ExchangeRateSGD = "Q4";
    public const string TotalAmountRMB = "Q5";

    // Card Inquiry Response Tags
    public const string CardNumberHash = "CH";
    public const string CardNumberMasked = "CM";
    public const string CardNumberWithPAN = "CN";
    public const string CardType = "CT";
    public const string EntryMode = "EM";
    public const string ICNumber = "IC";
    public const string PayerName = "PN";

    public const int TagValueLength = 3; // 3 bytes


}