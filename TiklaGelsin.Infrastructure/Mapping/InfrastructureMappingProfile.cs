using AutoMapper;
using TiklaGelsin.Domain.Entities;
using TiklaGelsin.Infrastructure.Persistence;

namespace TiklaGelsin.Infrastructure.Mapping
{
    public class InfrastructureMappingProfile : Profile
    {
        public InfrastructureMappingProfile()
        {
            // Domain -> Entity
            CreateMap<Order, OrderEntity>().ReverseMap();
            
            CreateMap<User, UserEntity>().ReverseMap();
        }
    }
}
