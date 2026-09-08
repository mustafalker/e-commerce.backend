using System.Threading.Tasks;
using TiklaGelsin.Application.Interfaces;
using TiklaGelsin.Domain.Entities;
using TiklaGelsin.Infrastructure.Persistence;
using AutoMapper;

namespace TiklaGelsin.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task AddAsync(Order order)
        {
            var entity = _mapper.Map<OrderEntity>(order);
            await _dbContext.Orders.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
