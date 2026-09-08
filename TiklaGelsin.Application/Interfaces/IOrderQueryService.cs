using System.Collections.Generic;
using System.Threading.Tasks;
using TiklaGelsin.Application.DTOs;

namespace TiklaGelsin.Application.Interfaces
{
    public interface IOrderQueryService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    }
}
