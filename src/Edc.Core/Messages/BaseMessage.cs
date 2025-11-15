using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Represents the base class for all EDC messages exchanged with the terminal.
/// Provides common properties and methods for parsing and validating messages.
/// </summary>
public abstract class BaseMessage
{
    // Protected members
    protected byte[] _message = Array.Empty<byte>();
    protected char _senderIndicator;

    // Public properties

    /// <summary>
    /// Start-of-text byte (STX) used to mark the beginning of a message.
    /// </summary>
    public static byte STX => Constants.STX;

    /// <summary>
    /// Length of the data portion of the message, parsed from BCD bytes.
    /// </summary>
    public int DataLength => BCDConverter.FromBCD(_message.AsSpan(1, 2).ToArray());

    /// <summary>
    /// Indicator of the sender (POS or terminal).
    /// </summary>
    public char SenderIndicator
    {
        get { return (char)_senderIndicator; }
        protected set { _senderIndicator = value; }
    }

    /// <summary>
    /// Transaction type of the message.
    /// </summary>
    public byte TransactionType => _message[DataFieldIndex.TransactionType];

    /// <summary>
    /// Version of the message format.
    /// </summary>
    public static string MessageVersion => Constants.MESSAGE_VERSION_V18;

    /// <summary>
    /// Complete raw message bytes including STX, ETX, and LRC.
    /// </summary>
    public byte[] Message => _message;

    /// <summary>
    /// Data portion of the message (excludes STX, ETX, and LRC).
    /// </summary>
    public byte[] Data => _message[3..^3];

    /// <summary>
    /// End-of-text byte (ETX) used to mark the end of a message.
    /// </summary>
    public static byte ETX => Constants.ETX;

    /// <summary>
    /// Longitudinal Redundancy Check (LRC) byte used for message integrity validation.
    /// </summary>
    public byte LRC => _message[^1];

    /// <summary>
    /// Validates the LRC of the message to ensure data integrity.
    /// </summary>
    /// <returns>True if the LRC matches the calculated value; otherwise, false.</returns>
    public bool IsValidLRC()
    {
        byte calculatedLRC = LRCCalculator.Calculate(
            _message[1..^1] // Exclude STX and LRC
        );
        return calculatedLRC == LRC;
    }
}