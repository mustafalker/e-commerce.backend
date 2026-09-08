using System.Threading.Tasks;
using TiklaGelsin.Application.Interfaces;
using TiklaGelsin.Domain.Entities;
using TiklaGelsin.Infrastructure.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace TiklaGelsin.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var entity = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (entity == null) return null;

            return _mapper.Map<User>(entity);
        }

        public async Task AddAsync(User user)
        {
            var entity = _mapper.Map<UserEntity>(user);
            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            var entity = await _dbContext.Users.FindAsync(user.Id);
            if (entity != null)
            {
                // Mevcut entity üzerinde alanları güncelliyoruz (EF tracking için)
                _mapper.Map(user, entity);
                _dbContext.Users.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
