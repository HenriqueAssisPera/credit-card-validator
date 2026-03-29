using CreditCardValidator.Data;
using CreditCardValidator.Entities;
using CreditCardValidator.Events;
using CreditCardValidator.Exceptions;
using CreditCardValidator.Validators;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreditCardValidator.Features.RegisterCard;

public class RegisterCardCommandHandler : IRequestHandler<RegisterCardCommand, RegisterCardResponse>
{
    private readonly CardValidator _cardValidator;
    private readonly AppDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<RegisterCardCommandHandler> _logger;

    public RegisterCardCommandHandler(
        CardValidator cardValidator,
        AppDbContext dbContext,
        IPublishEndpoint publishEndpoint,
        ILogger<RegisterCardCommandHandler> logger)
    {
        _cardValidator = cardValidator;
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<RegisterCardResponse> Handle(RegisterCardCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _cardValidator.Validate(request.CardNumber);
        var brandName = validationResult.Brand.ToString().ToUpperInvariant();

        if (!validationResult.IsValid)
        {
            _logger.LogInformation("Card validation failed for brand {Brand}.", brandName);

            return new RegisterCardResponse
            {
                IsValid = false,
                Brand = brandName,
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

        try
        {
            _dbContext.Cards.Add(card);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while registering card for cardholder {Cardholder}.", request.FullName);
            throw new CardRegistrationException("Failed to register the card due to a database error.", ex);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Card registration was cancelled for cardholder {Cardholder}.", request.FullName);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while persisting card for cardholder {Cardholder}.", request.FullName);
            throw new DatabaseUnavailableException("The database is currently unavailable. Please try again later.", ex);
        }

        await _publishEndpoint.Publish(new CardRegisteredEvent
        {
            CardId = card.Id,
            Brand = brandName,
            CardholderName = card.CardholderName,
            RegisteredAt = card.CreatedAt
        }, cancellationToken);

        _logger.LogInformation("Card registered and event published for cardholder {Cardholder} with brand {Brand}.", request.FullName, brandName);

        return new RegisterCardResponse
        {
            IsValid = true,
            Brand = brandName,
            Message = "Card validated and registered successfully."
        };
    }
}