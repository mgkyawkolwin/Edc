namespace Edc.Core.Common;

/// <summary>
/// Defines standard response codes returned by the EDC client or terminal.
/// These codes represent the outcome of transactions or indicate intermediate status updates.
/// </summary>
public static class ResponseCodes
{
    /// <summary>Transaction approved.</summary>
    public const string APPROVED = "00";

    /// <summary>Transaction aborted.</summary>
    public const string TRANSACTION_ABORTED = "TA";

    /// <summary>Transaction timed out.</summary>
    public const string TIMEOUT = "TO";

    /// <summary>Message format error.</summary>
    public const string FORMAT_ERROR = "FE";

    /// <summary>Invalid card.</summary>
    public const string INVALID_CARD = "IC";

    /// <summary>Card expired.</summary>
    public const string EXPIRED_CARD = "EC";

    /// <summary>Record not found.</summary>
    public const string NOT_FOUND = "NF";

    /// <summary>Batch is empty.</summary>
    public const string EMPTY_BATCH = "NU";

    /// <summary>Transaction not allowed.</summary>
    public const string NOT_ALLOWED = "IN";

    /// <summary>Unsupported card type.</summary>
    public const string UNSUPPORTED_CARD = "NS";

    /// <summary>Invalid response received.</summary>
    public const string INVALID_RESPONSE = "IR";

    /// <summary>Unable to void transaction.</summary>
    public const string UNABLE_TO_VOID = "VD";

    /// <summary>Void not possible.</summary>
    public const string VOID_NOT_POSSIBLE = "VN";

    /// <summary>Transaction not available.</summary>
    public const string NOT_AVAILABLE = "NA";

    /// <summary>Duplicate reference number detected.</summary>
    public const string DUPLICATE_REFERENCE = "DU";

    /// <summary>Cash-only transaction.</summary>
    public const string CASH_ONLY = "CA";

    /// <summary>General error occurred.</summary>
    public const string GENERAL_ERROR = "ER";

    /// <summary>Communication failure with terminal or host.</summary>
    public const string COMMUNICATION_FAILS = "CF";

    /// <summary>Chip card was swiped instead of inserted.</summary>
    public const string CHIP_CARD_SWIPED = "UC";

    /// <summary>Transaction amount exceeds limit.</summary>
    public const string AMOUNT_EXCEED_LIMIT = "AL";

    /// <summary>User selection required to continue.</summary>
    public const string USER_SELECTION_REQUIRED = "UI";

    // Status update codes

    /// <summary>Waiting for manual input from user (e.g., card number or PIN).</summary>
    public const string WAITING_MANUAL_INPUT = "W1";

    /// <summary>Waiting for connection to host.</summary>
    public const string WAITING_HOST_CONNECTION = "W2";

    /// <summary>Waiting for host response.</summary>
    public const string WAITING_HOST_RESPONSE = "W3";

    /// <summary>Waiting for signature confirmation.</summary>
    public const string WAITING_SIGNATURE = "W4";
}