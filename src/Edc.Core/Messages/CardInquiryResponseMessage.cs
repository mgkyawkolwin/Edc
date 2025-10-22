using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class CardInquiryResponseMessage : ResponseMessage
{
    private readonly byte[] _data;
    public CardInquiryResponseMessage(byte[] data)
    {
        _data = data;
    }

    public decimal GetAmount()
    {
        return Decimal.Parse(
            Encoding.ASCII.GetString(
                [.. _data.Take(new Range(DataFieldIndex.TransactionMessage.Response.Amount, DataFieldLength.Amount))]
            )
        );
    }

    public override byte[] GetData()
    {
        // start picking after 3 bytes (STX + Data Length) and exclude last 3 bytes (ETX + LRC)
        return
        [
            .. _data.Take(new Range(3, _data.Length - 3))
        ];
    }

    public override int GetDataLength()
    {
        return GetData().Length;
    }

    public string GetEcrRefNo()
    {
        return Encoding.ASCII.GetString(
            [.. _data.Take(new Range(
                DataFieldIndex.TransactionMessage.Response.EcrRefNo,
                DataFieldLength.EcrRefNo
            ))]
        );
    }

    public byte GetLRC()
    {
        return _data[_data.Length - 1];
    }

    public override byte[] GetMessage()
    {
        return _data;
    }

    public override byte[] GetResponseCode()
    {
        return [.. _data.Take(new Range(21, 22))];
    }

    public bool GetSignatureNotRequiredIndicator()
    {
        return _data[DataFieldIndex.TransactionMessage.Response.SignatureNotRequiredIndicator] != 0;
    }

    public byte GetTransactionType()
    {
        return (byte)0;
    }

    public override bool IsValid()
    {
        return LRCCalculator.Verify(GetData(), GetLRC());
    }
}