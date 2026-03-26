using CreditCardValidator.Validators;
using MediatR;

namespace CreditCardValidator.Features.RegisterCard;

public class RegisterCardCommandHandler : IRequestHandler<RegisterCardCommand, RegisterCardResponse>
{
    private readonly CardValidator _cardValidator;

    public RegisterCardCommandHandler()
    {
        _cardValidator = new CardValidator();
    }

    public Task<RegisterCardResponse> Handle(RegisterCardCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _cardValidator.Validate(request.CardNumber);

        var response = new RegisterCardResponse
        {
            IsValid = validationResult.IsValid,
            Brand = validationResult.Brand,
            Message = validationResult.IsValid
                ? "Card validated successfully."
                : "Invalid card."
        };

        return Task.FromResult(response);
    }
}