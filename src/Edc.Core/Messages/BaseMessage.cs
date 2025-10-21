using Edc.Core.Common;

namespace Edc.Core.Messages;

public abstract class BaseMessage
{
    public byte STX { get; protected set; } = Constants.STX;
    public byte[] DataLength { get; protected set; } = new byte[2];
    public char SenderIndicator { get; protected set; }
    public byte TransactionType { get; protected set; }
    public string MessageVersion { get; protected set; } = Constants.MESSAGE_VERSION_V18;
    public byte ETX { get; protected set; } = Constants.ETX;
    public byte LRC { get; protected set; }

    public abstract byte[] GetMessage();

    public abstract byte[] GetData();

    public abstract int GetDataLength();
}