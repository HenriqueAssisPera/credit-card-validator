using CreditCardValidator.Validators;
using MediatR;

namespace CreditCardValidator.Features.RegisterCard
{
    public class RegisterCardResponse
    {
        public bool IsValid { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
   
}
