using System.Threading.Tasks;
using TiklaGelsin.Domain.Entities;
using TiklaGelsin.Domain.Interfaces;
using TiklaGelsin.Application.Interfaces;

namespace TiklaGelsin.Application.Strategies
{
    public class CreditCardPayment : IPaymentMethod
    {
        private readonly IExternalPaymentService _externalPaymentService;

        public CreditCardPayment(IExternalPaymentService externalPaymentService)
        {
            _externalPaymentService = externalPaymentService;
        }

        public string PaymentType => "CreditCard";

        public async Task<bool> ProcessPaymentAsync(Order order)
        {
            // Gerçek dünyada kredi kartı entegrasyonu (Iyzico vs.) kullanılır.
            bool success = await _externalPaymentService.PayAsync(order);
            
            if (success)
            {
                order.MarkAsPaid();
            }

            return success;
        }
    }
}
