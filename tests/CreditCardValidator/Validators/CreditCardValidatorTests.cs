using CreditCardValidator.Enums;
using CreditCardValidator.Validators;
using FluentAssertions;
using Xunit;

namespace CreditCardValidator.UnitTests;

public class CreditCardValidatorTests
{
    [Theory]
    [InlineData("4111111111111111", CardBrand.Visa, true)]
    [InlineData("4111111111111", CardBrand.Visa, false)]
    [InlineData("4012888888881881", CardBrand.Visa, true)]
    [InlineData("378282246310005", CardBrand.Amex, true)]
    [InlineData("6011111111111117", CardBrand.Discover, true)]
    [InlineData("5105105105105100", CardBrand.MasterCard, true)]
    [InlineData("5105105105105106", CardBrand.MasterCard, false)]
    [InlineData("9111111111111111", CardBrand.Unknown, false)]
    public void Should_validate_cards_correctly(string cardNumber, CardBrand expectedBrand, bool expectedIsValid)
    {
        var validator = new CardValidator();

        var result = validator.Validate(cardNumber);

        result.Brand.Should().Be(expectedBrand);
        result.IsValid.Should().Be(expectedIsValid);
    }
}