namespace Edc.Core.Common;

public static class DataFieldIndex
{
    public const int STX = 0;
    public const int DataLength = 1;
    public const int SenderIndicator = 3;
    public const int TransactionType = 4;
    public const int MessageVersion = 5;

    public static class ConnectionMessage
    {
        public static class Request
        {
        }
        public static class Response
        {
            public const int PosDateTime = 8;
            public const int PosID = 22;
            public const int ResponseCode = 40;
        }
    }

    public static class CardInquiryMessage
    {
        public static class Request
        {
        }
        public static class Response
        {
            public const int Amount = 28;
            public const int EcrRefNo = 8;
            public const int PrivateField = 42;
            public const int ResponseCode = 40;
        }
    }

    public static class PrintReceiptMessage
    {
        public static class Request
        {
            public const int PosDateTime = 8;
            public const int PosID = 22;
            public const int HostNumber = 28;
            public const int BlockNumber = 31;
            public const int InvoiceTraceNumber = 37;
        }
        public static class Response
        {
            public const int PosDateTime = 8;
            public const int PosID = 22;
            public const int HostNumber = 28;
            public const int BlockNumber = 31;
            public const int PrintingDataLength = 37;
            public const int ResponseCode = 40;
            public const int PrintingData = 42;
        }
    }

    public static class TransactionMessage
    {
        public static class Request
        {

            public const int EcrRefNo = 8;
            public const int Amount = 28;
            public const int TerminalRefNo = 40;
            public const int ETX = 41;
            public const int LRC = 42;

        }
        public static class Response
        {
            public const int EcrRefNo = 8;
            public const int Amount = 28;
            public const int ResponseCode = 40;
            public const int MerchantId = 42;
            public const int TerminalId = 57;
            public const int PAN = 65;
            public const int CardExpDate = 85;
            public const int ApprovalCode = 89;
            public const int CardLabel = 95;
            public const int RRN = 105;
            public const int DateTime = 117;
            public const int BatchNumber = 129;
            public const int CardType = 135;
            public const int PersonName = 137;
            public const int SignatureNotRequiredIndicator = 164;
            public const int EntryMode = 165;
            public const int TerminalRefNo = 167;
            public const int RedemptionAmount = 173;
            public const int NetAmount = 185;
            public const int PrivateField = 197;

        }
    }

    public static class SettlementMessage
    {
        public static class Request
        {
            public const int HostNumber = 28;
            public const int PosDateTime = 8;
            public const int PosID = 22;
            public const int Reserved = 31;
        }
        public static class Response
        {
            public const int HostNumber = 28;
            public const int PosDateTime = 8;
            public const int PosID = 22;
            public const int Reserved = 31;
            public const int ResponseCode = 40;
            public static class SettlementSummary
            {
                public const int HostCount = 42;
                public const int HostNumber = 44;
                public const int HostName = 46;
                public const int SettlementResult = 55;
                public const int TotalSaleCount = 56;
                public const int TotalSaleAmount = 59;
                public const int TotalRefundCount = 71;
                public const int TotalRefundAmount = 74;                
            }
        }
    }
}