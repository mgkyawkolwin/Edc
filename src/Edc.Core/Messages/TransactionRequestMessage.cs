using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class TransactionRequestMessage : RequestMessage
{
    private readonly decimal _amount;
    private readonly string _ecrRefNo = "";
    private readonly string _terminalRefNo;
    private readonly TransactionTypes _transactionType;

    public TransactionRequestMessage(string ecrRefNo, string terminalRefNo, TransactionTypes transactionType, decimal amount)
    {
        _ecrRefNo = ecrRefNo;
        _amount = amount;
        _terminalRefNo = terminalRefNo;
        _transactionType = transactionType;
    }

    public string GetEcrRefNo()
    {
        return _ecrRefNo;
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
            .. Encoding.ASCII.GetBytes(Constants.MESSAGE_VERSION_V19),
            .. Encoding.ASCII.GetBytes(Helper.GetPaddedEcrRefNo(_ecrRefNo)),
            .. Encoding.ASCII.GetBytes(Helper.GetPaddedAmount(_amount)),
            .. Encoding.ASCII.GetBytes(_terminalRefNo),
        ];
    }

    public override int GetDataLength()
    {
        return GetData().Length;
    }

    public string GetTerminalRefNo()
    {
        return _terminalRefNo;
    }
    
    public byte GetTransactionType()
    {
        return (byte)_transactionType;
    }

}