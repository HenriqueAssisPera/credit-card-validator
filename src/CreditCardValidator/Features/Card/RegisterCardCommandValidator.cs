using FluentValidation;

namespace CreditCardValidator.Features.RegisterCard
{
    public class RegisterCardCommandValidator : AbstractValidator<RegisterCardCommand>
    {
        public RegisterCardCommandValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required.")
                .Matches(@"^\d+$").WithMessage("Card number must contain only numeric characters.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Birth date is required.");

            RuleFor(x => x.BirthDate)
                .LessThan(DateTime.Today).WithMessage("Birth date must not be a future date.")
                .When(x => x.BirthDate.HasValue);
        }
    }
}
