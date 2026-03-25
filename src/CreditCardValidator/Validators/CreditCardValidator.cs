namespace CreditCardValidator.Validators;

public class CardValidator
{
    public CreditCardValidationResult Validate(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return new CreditCardValidationResult { IsValid = false, Brand = "UNKNOWN" };

        cardNumber = cardNumber.Trim();

        var brand = IdentifyBrand(cardNumber);
        var isValid = brand != "UNKNOWN" && IsValidLuhn(cardNumber);

        return new CreditCardValidationResult { IsValid = isValid, Brand = brand };
    }

    #region Single Responsibility Principle
    private static string IdentifyBrand(string cardNumber)
    {
        var length = cardNumber.Length;

        // AMEX
        if (length == 15 && (cardNumber.StartsWith("34") || cardNumber.StartsWith("37")))
            return "AMEX";

        // DISCOVER
        if (length == 16 && cardNumber.StartsWith("6011"))
            return "DISCOVER";

        // MASTERCARD
        if (length == 16 && cardNumber.Length >= 2
            && int.TryParse(cardNumber[..2], out var prefix)
            && prefix >= 51 && prefix <= 55)
            return "MASTERCARD";

        // VISA
        if ((length == 13 || length == 16) && cardNumber.StartsWith("4"))
            return "VISA";

        return "UNKNOWN";
    }

    private static bool IsValidLuhn(string cardNumber)
    {
        var sum = 0;
        var alternate = false;

        for (var i = cardNumber.Length - 1; i >= 0; i--)
        {
            if (!char.IsDigit(cardNumber[i]))
                return false;

            var digit = cardNumber[i] - '0';

            if (alternate)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }

            sum += digit;
            alternate = !alternate;
        }

        return sum % 10 == 0;
    }
    #endregion
}