using System.Threading.Tasks;
using TiklaGelsin.Domain.Entities;

namespace TiklaGelsin.Application.Interfaces
{
    public interface IExternalPaymentService
    {
        Task<bool> PayAsync(Order order);
    }
}
