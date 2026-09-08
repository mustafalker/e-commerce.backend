using System;
using System.Threading.Tasks;
using AutoMapper;
using TiklaGelsin.Application.Interfaces;
using TiklaGelsin.Domain.Entities;
using TiklaGelsin.Infrastructure.Persistence;

namespace TiklaGelsin.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            var entity = await _dbContext.Products.FindAsync(id);
            return entity == null ? null : _mapper.Map<Product>(entity);
        }

        public async Task AddAsync(Product product)
        {
            var entity = _mapper.Map<ProductEntity>(product);
            await _dbContext.Products.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
