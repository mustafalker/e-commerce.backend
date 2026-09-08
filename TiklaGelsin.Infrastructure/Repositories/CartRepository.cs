using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TiklaGelsin.Application.Interfaces;
using TiklaGelsin.Domain.Entities;
using TiklaGelsin.Infrastructure.Persistence;

namespace TiklaGelsin.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CartRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task AddItemAsync(CartItem item)
        {
            var entity = _mapper.Map<CartItemEntity>(item);
            await _dbContext.CartItems.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(CartItem item)
        {
            var entity = await _dbContext.CartItems.FindAsync(item.Id);
            if (entity != null)
            {
                _mapper.Map(item, entity);
                _dbContext.CartItems.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<CartItem?> GetItemAsync(Guid userId, Guid productId)
        {
            var entity = await _dbContext.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
                
            return entity == null ? null : _mapper.Map<CartItem>(entity);
        }

        public async Task<IEnumerable<CartItem>> GetCartByUserIdAsync(Guid userId)
        {
            var entities = await _dbContext.CartItems
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CartItem>>(entities);
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var items = await _dbContext.CartItems.Where(c => c.UserId == userId).ToListAsync();
            _dbContext.CartItems.RemoveRange(items);
            await _dbContext.SaveChangesAsync();
        }
    }
}
