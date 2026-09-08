using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiklaGelsin.Domain.Entities;

namespace TiklaGelsin.Application.Interfaces
{
    public interface ICartRepository
    {
        Task AddItemAsync(CartItem item);
        Task UpdateItemAsync(CartItem item);
        Task<CartItem?> GetItemAsync(Guid userId, Guid productId);
        Task<IEnumerable<CartItem>> GetCartByUserIdAsync(Guid userId);
        Task ClearCartAsync(Guid userId);
    }
}
