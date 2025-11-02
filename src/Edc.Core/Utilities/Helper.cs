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

    public static string GetZeroPaddedAmount(decimal amount)
    {
        long amountInt = (long)Math.Round(amount * 100, MidpointRounding.AwayFromZero);
        return amountInt.ToString().PadLeft(DataFieldLength.Amount, '0');
    }


    public static string GetZeroPaddedBlockNo(string blockNumber)
    {
        return Convert.ToString(blockNumber).PadLeft(DataFieldLength.BlockNumber, '0');
    }


    public static string GetZeroPaddedEcrRefNo(string ecrRefNo)
    {
        return ecrRefNo.PadRight(DataFieldLength.EcrRefNo, '0');
    }


    public static string GetZeroPaddedTerminalRefNo(string terminalRefNo)
    {
        return terminalRefNo.PadRight(DataFieldLength.EcrRefNo, '0');
    }


    public static string GetZeroPaddedHostNumber(string hostNumber)
    {
        return hostNumber.PadLeft(DataFieldLength.HostNumber, '0');
    }


    public static string GetSpacePaddedPosID(string posId)
    {
        return posId.PadLeft(DataFieldLength.PosID, Constants.SPACE_CHAR);
    }


    public static string GetZeroPaddedReceiptTraceNo(int receiptTraceNo)
    {
        return Convert.ToString(receiptTraceNo).PadLeft(DataFieldLength.ReceiptTraceNo, '0');
    }

}