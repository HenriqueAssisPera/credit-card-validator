namespace CreditCardValidator.Entities
{
    public class Card
    {
        public Guid Id { get; set; }
        public string CardholderName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
