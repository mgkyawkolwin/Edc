namespace Edc.Core.Common;

public static class Constants
{
    public const byte STX = 0x02;
    public const byte ETX = 0x03;
    public const byte ACK = 0x06;
    public const byte NAK = 0x15;

    public const int MAX_RETRY_COUNT = 3;
    public const int RESPONSE_TIMEOUT_MS = 120000; // 2 minutes
    public const int ACK_TIMEOUT_MS = 2000; // 2 seconds

    public const string MESSAGE_VERSION_V18 = "V18";
    public const string MESSAGE_VERSION_V19 = "V19";

    public const char SENDER_POS = 'P';
    public const char SENDER_TERMINAL = 'T';

    public const string DEFAULT_TERMINAL_REFERENCE_NUMBER = "000000";

    public const int RESPONSE_BUFFER_SIZE = 4906;
}