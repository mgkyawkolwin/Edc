using Edc.Core.Common;

namespace Edc.Core.Utilities
{
    /// <summary>
    /// Provides helper methods for formatting and parsing data fields used in EDC messages.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Parses a datetime string in the format "yyyyMMddHHmmss" into a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="datetime">The datetime string in "yyyyMMddHHmmss" format.</param>
        /// <returns>The corresponding <see cref="DateTime"/> object.</returns>
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

        /// <summary>
        /// Converts a decimal amount to a zero-padded string suitable for message fields.
        /// Amount is multiplied by 100 and rounded to nearest integer.
        /// </summary>
        /// <param name="amount">The transaction amount.</param>
        /// <returns>A zero-padded string representation of the amount.</returns>
        public static string GetZeroPaddedAmount(decimal amount)
        {
            long amountInt = (long)Math.Round(amount * 100, MidpointRounding.AwayFromZero);
            return amountInt.ToString().PadLeft(DataFieldLength.Amount, '0');
        }

        /// <summary>
        /// Returns a block number string padded with zeros to match the required length.
        /// </summary>
        public static string GetZeroPaddedBlockNo(string blockNumber)
        {
            return blockNumber.PadLeft(DataFieldLength.BlockNumber, '0');
        }

        /// <summary>
        /// Returns an ECR reference number padded with spaces to match the required length.
        /// </summary>
        public static string GetSpacePaddedEcrRefNo(string ecrRefNo)
        {
            return ecrRefNo.PadRight(DataFieldLength.EcrRefNo, Constants.SPACE_CHAR);
        }

        /// <summary>
        /// Returns a terminal reference number padded with zeros to match the required length.
        /// </summary>
        public static string GetZeroPaddedTerminalRefNo(string terminalRefNo)
        {
            return terminalRefNo.PadLeft(DataFieldLength.TerminalRefNo, '0');
        }

        /// <summary>
        /// Returns a host number padded with zeros to match the required length.
        /// </summary>
        public static string GetZeroPaddedHostNumber(string hostNumber)
        {
            return hostNumber.PadLeft(DataFieldLength.HostNumber, '0');
        }

        /// <summary>
        /// Returns a POS ID padded with spaces to match the required length.
        /// </summary>
        public static string GetSpacePaddedPosID(string posId)
        {
            return posId.PadLeft(DataFieldLength.PosID, Constants.SPACE_CHAR);
        }

        /// <summary>
        /// Returns a receipt trace number padded with zeros to match the required length.
        /// </summary>
        /// <param name="receiptTraceNo">The receipt trace number.</param>
        public static string GetZeroPaddedReceiptTraceNo(int receiptTraceNo)
        {
            return receiptTraceNo.ToString().PadLeft(DataFieldLength.ReceiptTraceNo, '0');
        }
    }
}