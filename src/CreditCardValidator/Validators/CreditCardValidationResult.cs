using CreditCardValidator.Enums;

namespace CreditCardValidator.Validators;

public class CreditCardValidationResult
{
    public bool IsValid { get; set; }
    public CardBrand Brand { get; set; }
}