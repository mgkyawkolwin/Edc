using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class SettlementRequestMessage : RequestMessage
{
    private readonly DateTime _dateTime;
    private readonly string _posId ;
    private readonly TransactionTypes _transactionType;

    public SettlementRequestMessage(DateTime dateTime, TransactionTypes transactionType, string posId = "")
    {
        _dateTime = dateTime;
        _posId = posId;
        _transactionType = transactionType;
    }

    public override byte[] GetMessage()
    {
        byte[] data = GetData();

        return
        [
            base.STX,
            .. BCDConverter.ToBCD(data.Length),
            .. data,
            base.ETX,
            LRCCalculator.Calculate(data)
        ];
    }

    public override byte[] GetData()
    {
        return
        [
            (byte)Constants.SENDER_POS,
            (byte) _transactionType,
            .. Encoding.ASCII.GetBytes(Constants.MESSAGE_VERSION_V19)
        ];
    }

    public override int GetDataLength()
    {
        return GetData().Length;
    }
    
    public byte GetTransactionType()
    {
        return (byte)_transactionType;
    }

}