using System;

namespace Edc.Core.Utilities
{
    /// <summary>
    /// Provides methods to calculate and verify Longitudinal Redundancy Check (LRC) values for message integrity.
    /// </summary>
    public static class LRCCalculator
    {
        /// <summary>
        /// Calculates the LRC (Longitudinal Redundancy Check) for the given byte array.
        /// LRC is computed by performing a bitwise XOR over all bytes.
        /// </summary>
        /// <param name="data">The byte array for which to calculate the LRC.</param>
        /// <returns>The calculated LRC byte. Returns 0 if <paramref name="data"/> is null or empty.</returns>
        public static byte Calculate(byte[] data)
        {
            if (data == null || data.Length == 0)
                return 0;

            byte lrc = 0;
            foreach (byte b in data)
            {
                lrc ^= b;
            }

            // Debug output for client verification (optional, can be removed in production)
            Console.WriteLine("Client Calculated LRC: " + lrc.ToString("X2"));
            return lrc;
        }

        /// <summary>
        /// Verifies that the LRC for the given data matches the expected LRC value.
        /// </summary>
        /// <param name="data">The byte array to verify.</param>
        /// <param name="expectedLRC">The expected LRC value.</param>
        /// <returns><c>true</c> if the calculated LRC matches <paramref name="expectedLRC"/>; otherwise, <c>false</c>.</returns>
        public static bool Verify(byte[] data, byte expectedLRC)
        {
            return Calculate(data) == expectedLRC;
        }
    }
}