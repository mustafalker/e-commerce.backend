using System.Threading.Tasks;
using TiklaGelsin.Domain.Entities;
using TiklaGelsin.Domain.Interfaces;

namespace TiklaGelsin.Application.Strategies
{
    public class CashOnDeliveryPayment : IPaymentMethod
    {
        public string PaymentType => "CashOnDelivery";

        public Task<bool> ProcessPaymentAsync(Order order)
        {
            // Kapıda ödeme olduğu için ödeme anında gerçekleşmiyor, ama sipariş alınabiliyor
            // IsPaid false kalabilir veya sipariş tamamlandığında true yapılır. 
            // Burada şimdilik ödeme bekliyor diyoruz, başarılı dönüyoruz.
            return Task.FromResult(true);
        }
    }
}
