using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages;

public class TransactionRequestMessage : RequestMessage
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ecrRefNo"></param>
    /// <param name="amount"></param>
    /// <param name="transactionType"></param>
    /// <param name="terminalRefNo">
    /// For VOID and TIP ADJUST transaction, it will be
    /// terminal reference number of transaction to be voided/adjusted.
    /// For other type of transaction, fill in with zeroes.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public TransactionRequestMessage(string ecrRefNo, decimal amount, TransactionTypes transactionType, string terminalRefNo = Constants.EMPTY_TERMINAL_REF_NO)
    {
        if (ecrRefNo.Length > 20) throw new ArgumentOutOfRangeException(nameof(ecrRefNo), "ecrRefNo length should be <= 20");
        if (terminalRefNo.Length > 20) throw new ArgumentOutOfRangeException(nameof(terminalRefNo), "terminalRefNo length should be <= 6");

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

    public string EcrRefNo => Encoding.ASCII.GetString(
        _message[DataFieldIndex.TransactionMessage.Request.EcrRefNo .. DataFieldLength.EcrRefNo]
    );

    public decimal Amount => Convert.ToDecimal(
        Encoding.ASCII.GetString(
            _message[DataFieldIndex.TransactionMessage.Request.Amount .. DataFieldLength.Amount]
        )
    ) / 100m;

    public string TerminalRefNo => Encoding.ASCII.GetString(
        _message[DataFieldIndex.TransactionMessage.Request.TerminalRefNo .. DataFieldLength.TerminalRefNo]
    );

}