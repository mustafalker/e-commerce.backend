using System;
using System.Threading.Tasks;
using TiklaGelsin.Domain.Entities;

namespace TiklaGelsin.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task AddAsync(Product product);
    }
}
