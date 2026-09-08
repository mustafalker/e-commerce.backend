using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiklaGelsin.Application.DTOs;

namespace TiklaGelsin.Application.Interfaces
{
    public interface ICartQueryService
    {
        Task<IEnumerable<CartItemDto>> GetCartItemsAsync(Guid userId);
    }
}
