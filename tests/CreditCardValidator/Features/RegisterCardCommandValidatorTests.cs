using CreditCardValidator.Features.RegisterCard;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CreditCardValidator.UnitTests;

public class RegisterCardCommandValidatorTests
{
    private readonly RegisterCardCommandValidator _sut = new();

    #region Valid Command

    [Fact]
    public void Validate_ValidCommand_ShouldHaveNoErrors()
    {
        var command = new RegisterCardCommand
        {
            CardNumber = "4111111111111111",
            FullName = "John Doe",
            BirthDate = new DateTime(1990, 1, 1)
        };

        var result = _sut.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion

    #region CardNumber Validation

    [Fact]
    public void Validate_EmptyCardNumber_ShouldHaveErrorWithMessage()
    {
        var command = new RegisterCardCommand
        {
            CardNumber = string.Empty,
            FullName = "John Doe",
            BirthDate = new DateTime(1990, 1, 1)
        };

        var result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.CardNumber)
              .WithErrorMessage("Card number is required.");
    }

    [Theory]
    [InlineData("4111-1111-1111-1111")]
    [InlineData("4111 1111 1111 1111")]
    [InlineData("ABCDEFGHIJKLMNOP")]
    [InlineData("41111111111111x1")]
    public void Validate_NonNumericCardNumber_ShouldHaveErrorWithMessage(string cardNumber)
    {
        var command = new RegisterCardCommand
        {
            CardNumber = cardNumber,
            FullName = "John Doe",
            BirthDate = new DateTime(1990, 1, 1)
        };

        var result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.CardNumber)
              .WithErrorMessage("Card number must contain only numeric characters.");
    }

    #endregion

    #region FullName Validation

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_EmptyOrNullFullName_ShouldHaveErrorWithMessage(string? fullName)
    {
        var command = new RegisterCardCommand
        {
            CardNumber = "4111111111111111",
            FullName = fullName!,
            BirthDate = new DateTime(1990, 1, 1)
        };

        var result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FullName)
              .WithErrorMessage("Full name is required.");
    }

    #endregion

    #region BirthDate Validation

    [Fact]
    public void Validate_NullBirthDate_ShouldHaveErrorWithMessage()
    {
        var command = new RegisterCardCommand
        {
            CardNumber = "4111111111111111",
            FullName = "John Doe",
            BirthDate = null
        };

        var result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.BirthDate)
              .WithErrorMessage("Birth date is required.");
    }

    [Fact]
    public void Validate_FutureBirthDate_ShouldHaveErrorWithMessage()
    {
        var command = new RegisterCardCommand
        {
            CardNumber = "4111111111111111",
            FullName = "John Doe",
            BirthDate = DateTime.Today.AddDays(1)
        };

        var result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.BirthDate)
              .WithErrorMessage("Birth date must not be a future date.");
    }

    [Fact]
    public void Validate_TodayBirthDate_ShouldHaveErrorWithMessage()
    {
        var command = new RegisterCardCommand
        {
            CardNumber = "4111111111111111",
            FullName = "John Doe",
            BirthDate = DateTime.Today
        };

        var result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.BirthDate)
              .WithErrorMessage("Birth date must not be a future date.");
    }

    [Fact]
    public void Validate_PastBirthDate_ShouldNotHaveError()
    {
        var command = new RegisterCardCommand
        {
            CardNumber = "4111111111111111",
            FullName = "John Doe",
            BirthDate = DateTime.Today.AddDays(-1)
        };

        var result = _sut.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.BirthDate);
    }

    #endregion
}
