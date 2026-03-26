namespace CreditCardValidator.Exceptions;

/// <summary>
/// Exception thrown when a card registration fails due to a conflict or constraint violation.
/// </summary>
public class CardRegistrationException : Exception
{
    public CardRegistrationException()
        : base("An error occurred while registering the card.")
    {
    }

    public CardRegistrationException(string message)
        : base(message)
    {
    }

    public CardRegistrationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
