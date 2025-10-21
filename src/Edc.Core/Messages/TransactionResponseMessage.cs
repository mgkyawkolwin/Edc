using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class TransactionResponseMessage : ResponseMessage
{
    private readonly byte[] _data;
    public TransactionResponseMessage(byte[] data)
    {
        _data = data;
    }

    public byte[] GetApprovalCode()
    {
        return [.. _data.Take(new Range(90, 95))];
    }

    public byte[] GetCardExpiryDate()
    {
        throw new NotImplementedException();
    }

    public byte[] GetCardLabel()
    {
        return [.. _data.Take(new Range(96, 105))];
    }

    public override byte[] GetData()
    {
        return
        [
            .. _data.Take(new Range(3, _data.Length - 3))
        ];
    }

    public override int GetDataLength()
    {
        return GetData().Length;
    }

    public byte GetLRC()
    {
        return _data[_data.Length - 1];
    }

    public override byte[] GetMessage()
    {
        return _data;
    }

    public byte[] GetPAN()
    {
        throw new NotImplementedException();
    }
    
    public override byte[] GetResponseCode()
    {
        return [.. _data.Take(new Range(21, 22))];
    }

    public override bool IsValid()
    {
        return LRCCalculator.Verify(GetData(), GetLRC());
    }
}