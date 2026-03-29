namespace CreditCardValidator.Events;

public record CardRegisteredEvent
{
    public Guid CardId { get; init; }
    public string Brand { get; init; } = string.Empty;
    public string CardholderName { get; init; } = string.Empty;
    public DateTime RegisteredAt { get; init; }
}
