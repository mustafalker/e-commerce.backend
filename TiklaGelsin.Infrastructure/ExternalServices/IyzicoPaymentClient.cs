using System.Threading.Tasks;
using TiklaGelsin.Application.Interfaces;
using TiklaGelsin.Domain.Entities;

namespace TiklaGelsin.Infrastructure.ExternalServices
{
    public class IyzicoPaymentClient : IExternalPaymentService
    {
        public Task<bool> PayAsync(Order order)
        {
            // Gerçek dünyada burada Iyzico API'sine istek atılır.
            // Örnek olarak başarılı kabul ediyoruz.
            return Task.FromResult(true);
        }
    }
}
