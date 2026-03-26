using CreditCardValidator.Data;
using CreditCardValidator.Entities;
using CreditCardValidator.Validators;
using MediatR;

namespace CreditCardValidator.Features.RegisterCard;

public class RegisterCardCommandHandler : IRequestHandler<RegisterCardCommand, RegisterCardResponse>
{
    private readonly CardValidator _cardValidator;
    private readonly AppDbContext _dbContext;

    public RegisterCardCommandHandler(CardValidator cardValidator, AppDbContext dbContext)
    {
        _cardValidator = cardValidator;
        _dbContext = dbContext;
    }

    public async Task<RegisterCardResponse> Handle(RegisterCardCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _cardValidator.Validate(request.CardNumber);

        if (!validationResult.IsValid)
        {
            return new RegisterCardResponse
            {
                IsValid = false,
                Brand = validationResult.Brand,
                Message = "Invalid card."
            };
        }

        var card = new Card
        {
            Id = Guid.NewGuid(),
            CardholderName = request.FullName,
            BirthDate = request.BirthDate!.Value,
            Brand = validationResult.Brand,
            CardNumber = request.CardNumber,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Cards.Add(card);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new RegisterCardResponse
        {
            IsValid = true,
            Brand = validationResult.Brand,
            Message = "Card validated and registered successfully."
        };
    }
}