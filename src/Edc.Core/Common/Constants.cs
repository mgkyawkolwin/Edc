namespace Edc.Core.Common;

public static class Constants
{
    public const byte STX = 0x02;
    public const byte ETX = 0x03;
    public const byte ACK = 0x06;
    public const byte NAK = 0x15;
    public const string DATETIME_FORMAT = "yyyyMMddHHmmss";
    public const string EMPTY_POS_ID = "\x20\x20\x20\x20\x20\x20";
    public const string EMPTY_RESERVED_FIELD = "\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20\x20";
    public const string EMPTY_TERMINAL_REF_NO = "\x20\x20\x20\x20\x20\x20";
    public const string EMPTY_RECEIPT_TRACE_RESERVED_FIELD = "\x20\x20\x20\x20\x20\x20";
    public const int MAX_RETRY_COUNT = 3;
    public const int RESPONSE_TIMEOUT_MS = 150000; // 2 minutes
    public const int ACK_TIMEOUT_MS = 150000; // 2 seconds

    public const string MESSAGE_VERSION_V18 = "V18";

    public const char SENDER_POS = 'P';
    public const char SENDER_TERMINAL = 'T';
    public const char SPACE_CHAR = '\x20';

    public const string DEFAULT_TERMINAL_REFERENCE_NUMBER = "000000";

    public const int RESPONSE_BUFFER_SIZE = 4906;
}