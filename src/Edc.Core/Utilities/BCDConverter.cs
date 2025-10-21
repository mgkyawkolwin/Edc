namespace Edc.Core.Utilities;

public static class BCDConverter
{
    public static byte[] ToBCD(int value)
    {
        if (value < 0 || value > 9999)
            throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 9999");

        string digits = value.ToString("D4");   // always 4 digits
        int d1 = digits[0] - '0';
        int d2 = digits[1] - '0';
        int d3 = digits[2] - '0';
        int d4 = digits[3] - '0';

        byte b1 = (byte)((d1 << 4) | d2); // high byte: digits 1–2
        byte b2 = (byte)((d3 << 4) | d4); // low  byte: digits 3–4
        return new[] { b1, b2 };
    }

    public static int FromBCD(byte[] bcd)
    {
        if (bcd == null || bcd.Length != 2)
            throw new ArgumentException("BCD array must have exactly 2 bytes");

        int d1 = (bcd[0] >> 4) & 0x0F;
        int d2 = bcd[0] & 0x0F;
        int d3 = (bcd[1] >> 4) & 0x0F;
        int d4 = bcd[1] & 0x0F;

        return d1 * 1000 + d2 * 100 + d3 * 10 + d4;
    }
}