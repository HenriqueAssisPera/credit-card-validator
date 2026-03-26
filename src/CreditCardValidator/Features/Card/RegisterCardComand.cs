using MediatR;

namespace CreditCardValidator.Features.RegisterCard
{
    public class RegisterCardCommand : IRequest<RegisterCardResponse>
    {
        public string CardNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
    }
}
