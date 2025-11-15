namespace Edc.Core.Exceptions;

/// <summary>
/// Exception thrown when a transaction request is not acknowledged by the terminal.
/// This usually occurs when the terminal does not return an ACK control code
/// after sending a request message.
/// </summary>
public class NotAcknowledgedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotAcknowledgedException"/> class.
    /// </summary>
    public NotAcknowledgedException()
        : base("The terminal did not acknowledge the request.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotAcknowledgedException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public NotAcknowledgedException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotAcknowledgedException"/> class with a specified 
    /// error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public NotAcknowledgedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}