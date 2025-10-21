namespace Edc.Core.Common;

public static class ResponseCodes
{
    public const string APPROVED = "00";
    public const string TRANSACTION_ABORTED = "TA";
    public const string TIMEOUT = "TO";
    public const string FORMAT_ERROR = "FE";
    public const string INVALID_CARD = "IC";
    public const string EXPIRED_CARD = "EC";
    public const string NOT_FOUND = "NF";
    public const string EMPTY_BATCH = "NU";
    public const string NOT_ALLOWED = "IN";
    public const string UNSUPPORTED_CARD = "NS";
    public const string INVALID_RESPONSE = "IR";
    public const string UNABLE_TO_VOID = "VD";
    public const string VOID_NOT_POSSIBLE = "VN";
    public const string NOT_AVAILABLE = "NA";
    public const string DUPLICATE_REFERENCE = "DU";
    public const string CASH_ONLY = "CA";
    public const string GENERAL_ERROR = "ER";
    public const string COMMUNICATION_FAILS = "CF";
    public const string CHIP_CARD_SWIPED = "UC";
    public const string AMOUNT_EXCEED_LIMIT = "AL";
    public const string USER_SELECTION_REQUIRED = "UI";

    // Status update codes
    public const string WAITING_MANUAL_INPUT = "W1";
    public const string WAITING_HOST_CONNECTION = "W2";
    public const string WAITING_HOST_RESPONSE = "W3";
    public const string WAITING_SIGNATURE = "W4";

    public static bool IsApproved(string responseCode) => responseCode == APPROVED;
    public static bool IsError(string responseCode) => responseCode != APPROVED;
}