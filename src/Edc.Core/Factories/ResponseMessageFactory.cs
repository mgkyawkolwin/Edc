using Edc.Core.Common;
using Edc.Core.Messages;

namespace Edc.Core.Factories;

public static class ResponseMessageFactory
{
    public static ResponseMessage CreateResponseMessage(byte[] data)
    {
        switch ((TransactionTypes)data[DataFieldIndex.TransactionType])
        {
            case TransactionTypes.SALE_FULL_PAYMENT:
                return new TransactionResponseMessage(data);
            case TransactionTypes.CONNECTION_TEST:
                return new ConnectionResponseMessage(data);
            case TransactionTypes.CARD_ENQUIRY:
                return new CardInquiryResponseMessage(data);
            case TransactionTypes.CARD_ENQUIRY_BEFORE_SALES:
                return new CardInquiryResponseMessage(data);
            default:
                throw new NotSupportedException($"Unsupported transaction type: {data[4]:X2}");
        }
    }
}