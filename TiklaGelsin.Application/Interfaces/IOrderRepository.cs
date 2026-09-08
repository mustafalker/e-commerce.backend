using System.Threading.Tasks;
using TiklaGelsin.Domain.Entities;

namespace TiklaGelsin.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
    }
}
