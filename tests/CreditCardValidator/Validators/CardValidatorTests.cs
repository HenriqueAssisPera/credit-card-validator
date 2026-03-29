using CreditCardValidator.Enums;
using CreditCardValidator.Validators;
using FluentAssertions;
using Xunit;

namespace CreditCardValidator.UnitTests;

public class CardValidatorTests
{
    private readonly CardValidator _sut = new();

    #region Valid Cards — Luhn + Brand Match

    [Theory]
    [InlineData("4111111111111111", CardBrand.Visa)]
    [InlineData("4012888888881881", CardBrand.Visa)]
    public void Validate_ValidVisaCard_ShouldReturnIsValidTrueAndBrandVisa(string cardNumber, CardBrand expectedBrand)
    {
        var result = _sut.Validate(cardNumber);

        result.IsValid.Should().BeTrue();
        result.Brand.Should().Be(expectedBrand);
    }

    [Fact]
    public void Validate_ValidAmexCard_ShouldReturnIsValidTrueAndBrandAmex()
    {
        var result = _sut.Validate("378282246310005");

        result.IsValid.Should().BeTrue();
        result.Brand.Should().Be(CardBrand.Amex);
    }

    [Fact]
    public void Validate_ValidDiscoverCard_ShouldReturnIsValidTrueAndBrandDiscover()
    {
        var result = _sut.Validate("6011111111111117");

        result.IsValid.Should().BeTrue();
        result.Brand.Should().Be(CardBrand.Discover);
    }

    [Fact]
    public void Validate_ValidMasterCard_ShouldReturnIsValidTrueAndBrandMasterCard()
    {
        var result = _sut.Validate("5105105105105100");

        result.IsValid.Should().BeTrue();
        result.Brand.Should().Be(CardBrand.MasterCard);
    }

    #endregion

    #region Invalid Cards — Luhn Failure (Brand Identified)

    [Fact]
    public void Validate_VisaCardWithInvalidLuhn_ShouldReturnIsValidFalseAndBrandVisa()
    {
        var result = _sut.Validate("4111111111111");

        result.IsValid.Should().BeFalse();
        result.Brand.Should().Be(CardBrand.Visa);
    }

    [Fact]
    public void Validate_MasterCardWithInvalidLuhn_ShouldReturnIsValidFalseAndBrandMasterCard()
    {
        var result = _sut.Validate("5105105105105106");

        result.IsValid.Should().BeFalse();
        result.Brand.Should().Be(CardBrand.MasterCard);
    }

    #endregion

    #region Unknown Brand

    [Fact]
    public void Validate_UnrecognizedPrefix_ShouldReturnIsValidFalseAndBrandUnknown()
    {
        var result = _sut.Validate("9111111111111111");

        result.IsValid.Should().BeFalse();
        result.Brand.Should().Be(CardBrand.Unknown);
    }

    #endregion

    #region Null, Empty and Whitespace Input

    [Fact]
    public void Validate_NullCardNumber_ShouldReturnIsValidFalseAndBrandUnknown()
    {
        var result = _sut.Validate(null!);

        result.IsValid.Should().BeFalse();
        result.Brand.Should().Be(CardBrand.Unknown);
    }

    [Fact]
    public void Validate_EmptyCardNumber_ShouldReturnIsValidFalseAndBrandUnknown()
    {
        var result = _sut.Validate(string.Empty);

        result.IsValid.Should().BeFalse();
        result.Brand.Should().Be(CardBrand.Unknown);
    }

    [Fact]
    public void Validate_WhitespaceCardNumber_ShouldReturnIsValidFalseAndBrandUnknown()
    {
        var result = _sut.Validate("   ");

        result.IsValid.Should().BeFalse();
        result.Brand.Should().Be(CardBrand.Unknown);
    }

    #endregion

    #region Non-Numeric Input

    [Theory]
    [InlineData("4111-1111-1111-1111")]
    [InlineData("4111 1111 1111 1111")]
    [InlineData("ABCDEFGHIJKLMNO")]
    [InlineData("41111111111111x1")]
    public void Validate_NonNumericCardNumber_ShouldReturnIsValidFalseAndBrandUnknown(string cardNumber)
    {
        var result = _sut.Validate(cardNumber);

        result.IsValid.Should().BeFalse();
        result.Brand.Should().Be(CardBrand.Unknown);
    }

    #endregion
}