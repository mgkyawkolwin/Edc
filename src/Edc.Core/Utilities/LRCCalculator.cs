namespace Edc.Core.Utilities;

public static class LRCCalculator
{
    public static byte Calculate(byte[] data)
    {
        if (data == null || data.Length == 0)
            return 0;

        byte lrc = 0;
        foreach (byte b in data)
        {
            lrc ^= b;
        }
        Console.WriteLine("Client Calculated LRC: " + lrc.ToString("X2"));
        return lrc;
    }

    public static bool Verify(byte[] data, byte expectedLRC)
    {
        return Calculate(data) == expectedLRC;
    }
}