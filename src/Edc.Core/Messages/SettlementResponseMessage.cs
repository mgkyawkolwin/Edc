using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class SettlementResponseMessage : ResponseMessage
{
    private readonly byte[] _data;
    public SettlementResponseMessage(byte[] data)
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

    public string GetApprovalCode()
    {
        return Encoding.ASCII.GetString(
            [.. _data.Take(new Range(
                DataFieldIndex.TransactionMessage.Response.ApprovalCode,
                DataFieldLength.ApprovalCode
            ))]
        );
    }

    public string GetBatchNumber()
    {
        return Encoding.ASCII.GetString(
            [.. _data.Take(new Range(
                DataFieldIndex.TransactionMessage.Response.BatchNumber,
                DataFieldLength.BatchNumber
            ))]
        );
    }

    public string GetCardExpiryDate()
    {
        return Encoding.ASCII.GetString(
            [.. _data.Take(new Range(
                DataFieldIndex.TransactionMessage.Response.CardExpDate,
                DataFieldLength.CardExpDate
            ))]
        );
    }

    public string GetCardLabel()
    {
        return Encoding.ASCII.GetString(
            [.. _data.Take(new Range(
                DataFieldIndex.TransactionMessage.Response.CardLabel,
                DataFieldLength.CardLabel
            ))]
        );
    }

    public string GetCardType()
    {
        return Encoding.ASCII.GetString(
            [.. _data.Take(new Range(
                DataFieldIndex.TransactionMessage.Response.CardType,
                DataFieldLength.CardType
            ))]
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

    public string GetDateTime()
    {
        return Encoding.ASCII.GetString(
            [.. _data.Take(new Range(
                DataFieldIndex.TransactionMessage.Response.DateTime,
                DataFieldLength.DateTime
            ))]
        );
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

    public string GetMerchantId()
    {
        return Encoding.ASCII.GetString(
            [.. _data.Take(new Range(
                DataFieldIndex.TransactionMessage.Response.MerchantId,
                DataFieldLength.MerchantId
            ))]
        );
    }

    public override byte[] GetMessage()
    {
        return _data;
    }

    public string GetPersonName()
    {
        return Encoding.ASCII.GetString(
            [.. _data.Take(new Range(
                DataFieldIndex.TransactionMessage.Response.PersonName,
                DataFieldLength.PersonName
            ))]
        );
    }

    public string GetPRN()
    {
        return Encoding.ASCII.GetString(
            [.. _data.Take(new Range(
                DataFieldIndex.TransactionMessage.Response.PRN,
                DataFieldLength.PRN
            ))]
        );
    }

    public override byte[] GetResponseCode()
    {
        return [.. _data.Take(new Range(21, 22))];
    }

    public bool GetSignatureNotRequiredIndicator()
    {
        return _data[DataFieldIndex.TransactionMessage.Response.SignatureNotRequiredIndicator] != 0;
    }

    public string GetTerminalId()
    {
        return Encoding.ASCII.GetString(
            [.. _data.Take(new Range(
                DataFieldIndex.TransactionMessage.Response.TerminalId,
                DataFieldLength.TerminalId
            ))]
        );
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