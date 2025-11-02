using Edc.Core.Common;

namespace Edc.Core.Utilities;

public static class Helper
{
    public static DateTime GetDateTime(string datetime)
    {
        int year = Convert.ToInt32(datetime.Substring(0, 4));
        int month = Convert.ToInt32(datetime.Substring(4, 2));
        int day = Convert.ToInt32(datetime.Substring(6, 2));
        int hour = Convert.ToInt32(datetime.Substring(8, 2));
        int min = Convert.ToInt32(datetime.Substring(10, 2));
        int sec = Convert.ToInt32(datetime.Substring(12, 2));
        return new DateTime(year, month, day, hour, min, sec);
    }

    public static string GetPaddedAmount(decimal amount)
    {
        long amountInt = (long)(amount * 100);
        string amountString = amountInt.ToString();
        return amountString.PadLeft(12, '0');
    }


    public static string GetPaddedBlockNo(string blockNumber)
    {
        return Convert.ToString(blockNumber).PadLeft(DataFieldLength.BlockNumber, '0');
    }


    public static string GetPaddedEcrRefNo(string ecrRefNo)
    {
        return new String(ecrRefNo).PadLeft(20, '0');
    }


    public static string GetZeroPaddedHostNumber(string hostNumber)
    {
        return hostNumber.PadLeft(DataFieldLength.HostNumber, '0');
    }


    public static string GetSpacePaddedPosID(string posId)
    {
        return posId.PadLeft(DataFieldLength.PosID, Constants.SPACE_CHAR);
    }


    public static string GetPaddedReceiptTraceNo(int receiptTraceNo)
    {
        return Convert.ToString(receiptTraceNo).PadLeft(6, '0');
    }

}