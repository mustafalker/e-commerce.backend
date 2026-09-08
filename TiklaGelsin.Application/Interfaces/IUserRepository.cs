using System.Threading.Tasks;
using TiklaGelsin.Domain.Entities;

namespace TiklaGelsin.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}
