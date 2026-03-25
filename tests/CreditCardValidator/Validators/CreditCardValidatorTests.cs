using CreditCardValidator.Validators;
using FluentAssertions;
using Xunit;

namespace CreditCardValidator.UnitTests;

public class CreditCardValidatorTests
{
    [Theory]
    [InlineData("4111111111111111", "VISA", true)]
    [InlineData("4111111111111", "VISA", false)]
    [InlineData("4012888888881881", "VISA", true)]
    [InlineData("378282246310005", "AMEX", true)]
    [InlineData("6011111111111117", "DISCOVER", true)]
    [InlineData("5105105105105100", "MASTERCARD", true)]
    [InlineData("5105105105105106", "MASTERCARD", false)]
    [InlineData("9111111111111111", "UNKNOWN", false)]
    public void Should_validate_cards_correctly(string cardNumber, string expectedBrand, bool expectedIsValid)
    {
        var validator = new CardValidator();

        var result = validator.Validate(cardNumber);

        result.Brand.Should().Be(expectedBrand);
        result.IsValid.Should().Be(expectedIsValid);
    }
}