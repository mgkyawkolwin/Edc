namespace Edc.Core.Common;


public static class HostResponseCodes
{
    public const string Approved = "00";
    public const string CallAuthorizer = "01";
    public const string CallBank = "02";
    public const string InvalidMerchantOrTerminal = "03";
    public const string PickUpCard = "04";
    public const string TransactionDecliend = "05";
    public const string PickUpFraudCard = "07";
    public const string InvalidTransaction = "12";
    public const string InvalidAmount = "13";
    public const string InvalidCard = "14";
    public const string ReEnterTransaction = "19";
    public const string UnableToLocateRecordOnFile = "25";
    public const string FormatError = "30";
    public const string BankNotSupportedBySwitch = "31";
    public const string ExpiredCard = "33";
    public const string PinTriesExceeded = "38";
    public const string CreditAmountNotFound = "39";
    public const string CallAuthCenterLostCard = "41";
    public const string CallAuthCenterStolenCard = "43";
    public const string TransactionDecliendXXX = "51";
    public const string CurrentAccountNotAvailable = "52";
    public const string SavingAccountNotAvailable = "53";
    public const string CardExpiredXXX = "54";
    public const string PINInvalid = "55";
    public const string TransactionNotPermitedToCardHolder = "57";
    public const string TransactionNotPermittedToTerminal = "58";
    public const string ExceedWithdrwalAmountLimit = "61";
    public const string RestrictedCard = "62";
    public const string IssuerOrSwitchInOperative = "91";
    public const string DuplicatedTransaction = "94";
    public const string SystemMalfunction = "96";
}