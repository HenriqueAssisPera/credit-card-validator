using CreditCardValidator.Validators;
using FluentAssertions;
using Xunit;

namespace CreditCardValidator.UnitTests;

public class CreditCardValidatorTests
{
    [Fact]
    public void Should_validate_visa_card_as_valid()
    {
        var validator = new CardValidator();

        var result = validator.Validate("4111111111111111");

        result.IsValid.Should().BeTrue();
        result.Brand.Should().Be("VISA");
    }
}