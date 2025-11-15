using System.Text;
using Edc.Core.Common;
using Edc.Core.Utilities;

namespace Edc.Core.Messages
{
    /// <summary>
    /// Represents a transaction status update response message received from the host.
    /// Contains information such as amount, ECR reference, and additional private fields.
    /// </summary>
    public class TransactionStatusUpdateResponseMessage : ResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionStatusUpdateResponseMessage"/> class.
        /// </summary>
        /// <param name="message">The raw byte array of the message received from the host.</param>
        public TransactionStatusUpdateResponseMessage(byte[] message)
        {
            _message = message;
        }

        /// <summary>
        /// Transaction amount sent in the status update.
        /// </summary>
        public decimal Amount => Convert.ToDecimal(
            Encoding.ASCII.GetString(
                _message.AsSpan(DataFieldIndex.TransactionStatusUpdateMessage.Response.Amount, DataFieldLength.Amount)
            )
        ) / 100m;

        /// <summary>
        /// Electronics Cash Register (ECR) reference number.
        /// </summary>
        public string EcrRefNo => Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.TransactionStatusUpdateMessage.Response.EcrRefNo, DataFieldLength.EcrRefNo)
        ).Trim();

        /// <summary>
        /// Additional private data in TLV (Tag-Length-Value) format.
        /// May contain zero or more TLV fields.
        /// </summary>
        public string PrivateField => Encoding.ASCII.GetString(
            _message[DataFieldIndex.TransactionStatusUpdateMessage.Response.PrivateField..^3]
        );

        /// <summary>
        /// Response code of the terminal. See <see cref="Edc.Core.Common.ResponseCodes"/> for full list.
        /// </summary>
        public override string ResponseCode => Encoding.ASCII.GetString(
            _message.AsSpan(DataFieldIndex.TransactionStatusUpdateMessage.Response.ResponseCode, DataFieldLength.ResponseCode)
        );
    }
}