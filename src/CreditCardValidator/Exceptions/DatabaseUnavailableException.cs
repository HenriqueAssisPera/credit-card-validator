namespace CreditCardValidator.Exceptions;

/// <summary>
/// Exception thrown when the database is unavailable or a persistence operation fails.
/// </summary>
public class DatabaseUnavailableException : Exception
{
    public DatabaseUnavailableException()
        : base("The database is currently unavailable. Please try again later.")
    {
    }

    public DatabaseUnavailableException(string message)
        : base(message)
    {
    }

    public DatabaseUnavailableException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
