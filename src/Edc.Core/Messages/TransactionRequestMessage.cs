using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class TransactionRequestMessage : RequestMessage
{

    /// <summary>
    /// TransactionRequestMessage Constructor.
    /// </summary>
    /// <param name="ecrRefNo">Electronics cash register (ECR) reference number.</param>
    /// <param name="amount">Transaction amount. For TIP ADJUST, this would be the tip amount to be added to the original base amount.</param>
    /// <param name="transactionType">Transaction type.</param>
    /// <param name="terminalRefNo">
    /// For VOID and TIP ADJUST transaction, it will be
    /// terminal reference number of transaction to be voided/adjusted.
    /// For other type of transaction, pass zero.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public TransactionRequestMessage(string ecrRefNo = "", decimal amount = 0, TransactionTypes transactionType = TransactionTypes.SALE_FULL_PAYMENT, string terminalRefNo = "")
    {
        if (ecrRefNo.Length > DataFieldLength.EcrRefNo) throw new ArgumentOutOfRangeException(nameof(ecrRefNo), "ecrRefNo length should be <= 20");
        if (terminalRefNo.Length > DataFieldLength.TerminalRefNo) throw new ArgumentOutOfRangeException(nameof(terminalRefNo), "terminalRefNo length should be <= 6");

        // Build the data field
        byte[] _data = new byte[] {
            (byte)SenderIndicator,
            (byte) transactionType,
        }
        .Concat(Encoding.ASCII.GetBytes(MessageVersion))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetZeroPaddedEcrRefNo(ecrRefNo)))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetZeroPaddedAmount(amount)))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetZeroPaddedTerminalRefNo(terminalRefNo))).ToArray();

        // Compute BCD
        byte[] bcd = BCDConverter.ToBCD(_data.Length);

        // Calculate LRC
        byte lrc = LRCCalculator.Calculate(
            Array.Empty<byte>().Concat(bcd).Concat(_data).Concat(new byte[] { ETX }).ToArray()
        );

        // Build the complete message
        _message = Array.Empty<byte>()
            .Concat(new byte[] { STX })
            .Concat(bcd)
            .Concat(_data)
            .Concat(new byte[] { ETX })
            .Concat(new byte[] { lrc })
            .ToArray();
    }

    /// <summary>
    /// Electronics cash register (ECR) reference number.
    /// Padded with SPACES, left aligned, if less than 20 digits. 
    /// This number will be printed on terminal receipt, if applicable.
    /// </summary>
    public string EcrRefNo => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Request.EcrRefNo, DataFieldLength.EcrRefNo)
    );

    /// <summary>
    /// Transaction amount.
    /// For TIP ADJUST, this should be the tip amount to be
    /// added to original base amount.
    /// </summary>
    public decimal Amount => Convert.ToDecimal(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.TransactionMessage.Request.Amount, DataFieldLength.Amount)
        )
    ) / 100m;

    /// <summary>
    /// For VOID and TIP ADJUST transaction, it will be
    /// terminal reference number of transaction to be
    /// voided/adjusted.
    /// For other type of transaction, filled with zeroes.
    /// </summary>
    public string TerminalRefNo => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Request.TerminalRefNo, DataFieldLength.TerminalRefNo)
    );

}