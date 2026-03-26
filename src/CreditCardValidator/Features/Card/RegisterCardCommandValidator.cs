using FluentValidation;

namespace CreditCardValidator.Features.RegisterCard
{
    public class RegisterCardCommandValidator : AbstractValidator<RegisterCardCommand>
    {
        public RegisterCardCommandValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Birth date is required");
        }
    }
}
