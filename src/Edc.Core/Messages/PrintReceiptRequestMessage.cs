using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class PrintReceiptRequestMessage : RequestMessage
{
    private readonly decimal _amount;
    private readonly string _ecrRefNo = "";
    private readonly string _terminalRefNo;
    private readonly TransactionTypes _transactionType;

    public PrintReceiptRequestMessage(string ecrRefNo, string terminalRefNo, TransactionTypes transactionType, decimal amount)
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

    // public override byte[] GetMessage()
    // {
    //     byte[] data = GetData();
    //     return (byte[])new byte[] { }.Concat(new[] { STX })
    //         .Concat(BCDConverter.ToBCD(data.Length))
    //         .Concat(data)
    //         .Concat(new[] { ETX })
    //         .Concat(new[] { LRCCalculator.Calculate(data) });
    // }

    // public override byte[] GetData()
    // {
    //     return (byte[])new byte[] {
    //         (byte)Constants.SENDER_POS,
    //         (byte) _transactionType,
    //     }
    //     .Concat(Encoding.ASCII.GetBytes(Constants.MESSAGE_VERSION_V18))
    //     .Concat(Encoding.ASCII.GetBytes(Helper.GetPaddedEcrRefNo(_ecrRefNo)))
    //     .Concat(Encoding.ASCII.GetBytes(Helper.GetPaddedAmount(_amount)))
    //     .Concat(Encoding.ASCII.GetBytes(_terminalRefNo));
    // }

    // public override int GetDataLength()
    // {
    //     return GetData().Length;
    // }

    public string GetTerminalRefNo()
    {
        return _terminalRefNo;
    }

    public byte GetTransactionType()
    {
        return (byte)_transactionType;
    }

}