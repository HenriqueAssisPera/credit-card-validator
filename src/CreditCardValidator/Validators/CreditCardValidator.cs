namespace CreditCardValidator.Validators;

public class CardValidator
{
    public CreditCardValidationResult Validate(string cardNumber)
    {
        return new CreditCardValidationResult
        {
            IsValid = false,
            Brand = "UNKNOWN"
        };
    }
}