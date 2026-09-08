using TiklaGelsin.Domain.Entities;
using System.Threading.Tasks;

namespace TiklaGelsin.Domain.Interfaces
{
    public interface IPaymentMethod
    {
        string PaymentType { get; }
        Task<bool> ProcessPaymentAsync(Order order);
    }
}
