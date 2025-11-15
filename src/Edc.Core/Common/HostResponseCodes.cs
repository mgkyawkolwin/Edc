namespace Edc.Core.Common;


/// <summary>
/// Defines standard response codes returned by the host/bank for EDC transactions.
/// These codes follow ISO-8583 conventions and indicate approval, errors, or required actions.
/// </summary>
public static class HostResponseCodes
{
    /// <summary>Transaction approved.</summary>
    public const string Approved = "00";

    /// <summary>Call the authorizer.</summary>
    public const string CallAuthorizer = "01";

    /// <summary>Call the bank.</summary>
    public const string CallBank = "02";

    /// <summary>Invalid merchant or terminal.</summary>
    public const string InvalidMerchantOrTerminal = "03";

    /// <summary>Pick up the card (retention).</summary>
    public const string PickUpCard = "04";

    /// <summary>Transaction declined.</summary>
    public const string TransactionDecliend = "05";

    /// <summary>Pick up card due to suspected fraud.</summary>
    public const string PickUpFraudCard = "07";

    /// <summary>Invalid transaction.</summary>
    public const string InvalidTransaction = "12";

    /// <summary>Invalid amount.</summary>
    public const string InvalidAmount = "13";

    /// <summary>Invalid card.</summary>
    public const string InvalidCard = "14";

    /// <summary>Re-enter transaction.</summary>
    public const string ReEnterTransaction = "19";

    /// <summary>Unable to locate record on file.</summary>
    public const string UnableToLocateRecordOnFile = "25";

    /// <summary>Message format error.</summary>
    public const string FormatError = "30";

    /// <summary>Bank not supported by switch.</summary>
    public const string BankNotSupportedBySwitch = "31";

    /// <summary>Card expired.</summary>
    public const string ExpiredCard = "33";

    /// <summary>PIN tries exceeded.</summary>
    public const string PinTriesExceeded = "38";

    /// <summary>Credit amount not found.</summary>
    public const string CreditAmountNotFound = "39";

    /// <summary>Call authorization center for lost card.</summary>
    public const string CallAuthCenterLostCard = "41";

    /// <summary>Call authorization center for stolen card.</summary>
    public const string CallAuthCenterStolenCard = "43";

    /// <summary>Transaction declined (alternate code).</summary>
    public const string DeclinedTransaction = "51";

    /// <summary>Current account not available.</summary>
    public const string CurrentAccountNotAvailable = "52";

    /// <summary>Savings account not available.</summary>
    public const string SavingAccountNotAvailable = "53";

    /// <summary>Card expired.</summary>
    public const string CardExpired = "54";

    /// <summary>Invalid PIN.</summary>
    public const string PINInvalid = "55";

    /// <summary>Transaction not permitted to cardholder.</summary>
    public const string TransactionNotPermitedToCardHolder = "57";

    /// <summary>Transaction not permitted to terminal.</summary>
    public const string TransactionNotPermittedToTerminal = "58";

    /// <summary>Exceeded withdrawal amount limit.</summary>
    public const string ExceedWithdrwalAmountLimit = "61";

    /// <summary>Restricted card.</summary>
    public const string RestrictedCard = "62";

    /// <summary>Issuer or switch inoperative.</summary>
    public const string IssuerOrSwitchInOperative = "91";

    /// <summary>Duplicated transaction.</summary>
    public const string DuplicatedTransaction = "94";

    /// <summary>System malfunction.</summary>
    public const string SystemMalfunction = "96";
}