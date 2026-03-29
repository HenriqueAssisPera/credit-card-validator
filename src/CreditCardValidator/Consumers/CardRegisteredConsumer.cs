using CreditCardValidator.Events;
using MassTransit;

namespace CreditCardValidator.Consumers;

public class CardRegisteredConsumer : IConsumer<CardRegisteredEvent>
{
    private readonly ILogger<CardRegisteredConsumer> _logger;

    public CardRegisteredConsumer(ILogger<CardRegisteredConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<CardRegisteredEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Event consumed — CardId: {CardId}, Brand: {Brand}, Cardholder: {Cardholder}, RegisteredAt: {RegisteredAt}",
            message.CardId,
            message.Brand,
            message.CardholderName,
            message.RegisteredAt);

        return Task.CompletedTask;
    }
}
