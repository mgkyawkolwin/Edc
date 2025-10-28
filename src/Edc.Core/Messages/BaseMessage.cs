using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public abstract class BaseMessage
{
    // Protected members
    protected byte[] _message = Array.Empty<byte>();
    protected byte[] _data = Array.Empty<byte>();
    private char _senderIndicator;

    // Public properties
    public static byte STX => Constants.STX;
    public int DataLength => BCDConverter.FromBCD(_message.AsSpan(1, 2).ToArray());
    public virtual char SenderIndicator 
    { 
        get { return (char)_senderIndicator; } 
        protected set { _senderIndicator = value; } 
    }
    public byte TransactionType => _data[DataFieldIndex.TransactionType];
    public static string MessageVersion => Constants.MESSAGE_VERSION_V18;

    public byte[] Message => _message;
    public byte[] Data => _message[3..^3]; // Exclude STX, ETX, LRC
    public static byte ETX => Constants.ETX;
    public byte LRC => _message[^1];

    // public abstract byte[] GetMessage();

    // public abstract byte[] GetData();

    // public abstract int GetDataLength();

    public bool IsValidLRC()
    {
        byte calculatedLRC = LRCCalculator.Calculate(
            _message[1..^1] // Exclude STX and LRC
        );
        return calculatedLRC == LRC;
    }
}