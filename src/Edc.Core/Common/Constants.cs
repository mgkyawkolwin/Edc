namespace Edc.Core.Common;

/// <summary>
/// Defines constants used throughout the EDC client and ISO-8583 message processing.
/// </summary>
public static class Constants
{
    /// <summary>Start of text byte (STX).</summary>
    public const byte STX = 0x02;

    /// <summary>End of text byte (ETX).</summary>
    public const byte ETX = 0x03;

    /// <summary>Acknowledge byte (ACK) sent by the terminal to confirm message reception.</summary>
    public const byte ACK = 0x06;

    /// <summary>Negative acknowledge byte (NAK) sent by the terminal to indicate message error.</summary>
    public const byte NAK = 0x15;

    /// <summary>Standard date-time format for message timestamps: "yyyyMMddHHmmss".</summary>
    public const string DATETIME_FORMAT = "yyyyMMddHHmmss";

    /// <summary>Empty POS ID placeholder (6 spaces).</summary>
    public const string EMPTY_POS_ID = "\x20\x20\x20\x20\x20\x20";

    /// <summary>Empty reserved field placeholder (20 spaces).</summary>
    public const string EMPTY_RESERVED_FIELD = "\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20";

    /// <summary>Empty terminal reference number placeholder (6 spaces).</summary>
    public const string EMPTY_TERMINAL_REF_NO = "\x20\x20\x20\x20\x20\x20";

    /// <summary>Empty receipt trace reserved field placeholder (6 spaces).</summary>
    public const string EMPTY_RECEIPT_TRACE_RESERVED_FIELD = "\x20\x20\x20\x20\x20\x20";

    /// <summary>Maximum retry attempts for sending messages.</summary>
    public const int MAX_RETRY_COUNT = 3;

    /// <summary>Default timeout for waiting for a response in milliseconds (2 minutes).</summary>
    public const int RESPONSE_TIMEOUT_MS = 150000; // 2 minutes

    /// <summary>Timeout for waiting for ACK/NAK in milliseconds (2 seconds).</summary>
    public const int ACK_TIMEOUT_MS = 150000; // 2 seconds

    /// <summary>Message version identifier for V1.8.</summary>
    public const string MESSAGE_VERSION_V18 = "V18";

    /// <summary>Sender type character for POS ('P').</summary>
    public const char SENDER_POS = 'P';

    /// <summary>Sender type character for Terminal ('T').</summary>
    public const char SENDER_TERMINAL = 'T';

    /// <summary>Space character constant.</summary>
    public const char SPACE_CHAR = '\x20';

    /// <summary>Default terminal reference number used in messages.</summary>
    public const string DEFAULT_TERMINAL_REFERENCE_NUMBER = "000000";

    /// <summary>Buffer size for receiving responses from the terminal.</summary>
    public const int RESPONSE_BUFFER_SIZE = 4906;
}