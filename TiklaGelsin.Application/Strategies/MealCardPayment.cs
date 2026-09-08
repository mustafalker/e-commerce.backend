using System.Threading.Tasks;
using TiklaGelsin.Domain.Entities;
using TiklaGelsin.Domain.Exceptions;
using TiklaGelsin.Domain.Interfaces;

namespace TiklaGelsin.Application.Strategies
{
    public class MealCardPayment : IPaymentMethod
    {
        public string PaymentType => "MealCard";

        public Task<bool> ProcessPaymentAsync(Order order)
        {
            // Yemek kartı limiti kontrolü vs. burada yapılabilir.
            // Örnek olarak sipariş tutarı 1000'den büyükse yemek kartı limiti yetersiz varsayalım
            if (order.TotalAmount > 1000)
            {
                throw new DomainException("Yemek kartı limiti yetersiz.");
            }

            order.MarkAsPaid();
            return Task.FromResult(true);
        }
    }
}
