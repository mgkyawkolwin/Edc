namespace Edc.Core.Utilities;

public static class Helper
{
    public static string GetPaddedEcrRefNo(string ecrRefNo)
    {
        return new String(ecrRefNo).PadLeft(20, '0');
    }

    public static string GetPaddedAmount(decimal amount)
    {
        long amountInt = (long)(amount * 100);
        return amountInt.ToString().PadLeft(12, '0');
    }

}