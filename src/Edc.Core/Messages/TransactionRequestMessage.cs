using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

/// <summary>
/// Represents a transaction request message sent from the POS to the terminal.
/// This includes SALE, VOID, REFUND, TIP ADJUST, and other transaction types.
/// </summary>
public class TransactionRequestMessage : RequestMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionRequestMessage"/> class.
    /// </summary>
    /// <param name="amount">
    /// Transaction amount. For TIP ADJUST, this should be the tip amount to add to the original base amount.
    /// </param>
    /// <param name="ecrRefNo">
    /// Electronic Cash Register (ECR) reference number. Padded with spaces if less than 20 characters.
    /// Printed on terminal receipt if applicable.
    /// </param>
    /// <param name="transactionType">Type of transaction (SALE, VOID, REFUND, TIP ADJUST, etc.).</param>
    /// <param name="terminalRefNo">
    /// Terminal reference number for VOID or TIP ADJUST transactions.
    /// For other transaction types, pass zeroes.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="ecrRefNo"/> exceeds 20 characters or <paramref name="terminalRefNo"/> exceeds 6 characters.
    /// </exception>
    public TransactionRequestMessage(
        decimal amount = 0,
        string ecrRefNo = "",
        TransactionTypes transactionType = TransactionTypes.SALE_FULL_PAYMENT,
        string terminalRefNo = "")
    {
        if (ecrRefNo.Length > DataFieldLength.EcrRefNo)
            throw new ArgumentOutOfRangeException(nameof(ecrRefNo), "ecrRefNo length should be <= 20");
        if (terminalRefNo.Length > DataFieldLength.TerminalRefNo)
            throw new ArgumentOutOfRangeException(nameof(terminalRefNo), "terminalRefNo length should be <= 6");

        // Build the data field
        byte[] _data = new byte[] {
            (byte)SenderIndicator,
            (byte) transactionType,
        }
        .Concat(Encoding.ASCII.GetBytes(MessageVersion))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetSpacePaddedEcrRefNo(ecrRefNo)))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetZeroPaddedAmount(amount)))
        .Concat(Encoding.ASCII.GetBytes(Helper.GetZeroPaddedTerminalRefNo(terminalRefNo)))
        .ToArray();

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
    /// Gets the Electronic Cash Register (ECR) reference number.
    /// Padded with spaces and left-aligned if shorter than 20 characters.
    /// </summary>
    public string EcrRefNo => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Request.EcrRefNo, DataFieldLength.EcrRefNo)
    );

    /// <summary>
    /// Gets the transaction amount.
    /// For TIP ADJUST transactions, this represents the tip amount to be added to the original base amount.
    /// </summary>
    public decimal Amount => Convert.ToDecimal(
        Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.TransactionMessage.Request.Amount, DataFieldLength.Amount)
        )
    ) / 100m;

    /// <summary>
    /// Gets the terminal reference number.
    /// Used for VOID and TIP ADJUST transactions.
    /// For other transactions, this is filled with zeroes.
    /// </summary>
    public string TerminalRefNo => Encoding.ASCII.GetString(
        _message.AsSpan(DataFieldIndex.TransactionMessage.Request.TerminalRefNo, DataFieldLength.TerminalRefNo)
    );
}