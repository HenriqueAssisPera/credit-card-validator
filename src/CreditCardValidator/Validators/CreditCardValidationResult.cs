namespace CreditCardValidator.Validators;

public class CreditCardValidationResult
{
    public bool IsValid { get; set; }
    public string Brand { get; set; } = string.Empty;
}