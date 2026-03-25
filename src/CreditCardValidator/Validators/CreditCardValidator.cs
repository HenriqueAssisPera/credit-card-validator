namespace CreditCardValidator.Validators;

public class CardValidator
{
    public CreditCardValidationResult Validate(string cardNumber)
    {
        #region null check and trim
        if (string.IsNullOrWhiteSpace(cardNumber))
            return new CreditCardValidationResult { IsValid = false, Brand = "UNKNOW" };

        cardNumber = cardNumber.Trim();
        #endregion

        if (cardNumber.StartsWith("4") && cardNumber.Length == 16)
        {
            return new CreditCardValidationResult { IsValid = true, Brand = "VISA" };
        }
        else if (cardNumber.StartsWith("4"))
        {
            return new CreditCardValidationResult { IsValid = false, Brand = "VISA" };
        }

        return new CreditCardValidationResult { IsValid = false, Brand = "UNKNOW" };

    }
}