namespace Edc.Core.Utilities
{
    /// <summary>
    /// Provides helper methods to convert between integers and BCD (Binary-Coded Decimal) representation.
    /// </summary>
    public static class BCDConverter
    {
        /// <summary>
        /// Converts an integer value (0–9999) to a 2-byte BCD representation.
        /// </summary>
        /// <param name="value">The integer value to convert. Must be between 0 and 9999 inclusive.</param>
        /// <returns>A 2-byte array representing the BCD of the value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the value is less than 0 or greater than 9999.</exception>
        public static byte[] ToBCD(int value)
        {
            if (value < 0 || value > 9999)
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 9999");

            string digits = value.ToString("D4"); // Always 4 digits, zero-padded
            int d1 = digits[0] - '0';
            int d2 = digits[1] - '0';
            int d3 = digits[2] - '0';
            int d4 = digits[3] - '0';

            byte b1 = (byte)((d1 << 4) | d2); // High byte: digits 1–2
            byte b2 = (byte)((d3 << 4) | d4); // Low byte: digits 3–4

            return new[] { b1, b2 };
        }

        /// <summary>
        /// Converts a 2-byte BCD array to its integer representation.
        /// </summary>
        /// <param name="bcd">A 2-byte array representing a BCD value.</param>
        /// <returns>The integer equivalent of the BCD value.</returns>
        /// <exception cref="ArgumentException">Thrown if the input array is null or not exactly 2 bytes long.</exception>
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
}