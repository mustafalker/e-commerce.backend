using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using TiklaGelsin.Application.DTOs;
using TiklaGelsin.Application.Interfaces;

namespace TiklaGelsin.Infrastructure.Services
{
    public class OrderQueryService : IOrderQueryService
    {
        private readonly string _connectionString;

        public OrderQueryService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                                ?? throw new System.ArgumentNullException("DefaultConnection");
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            using IDbConnection dbConnection = new NpgsqlConnection(_connectionString);
            string query = "SELECT \"Id\", \"UserId\", \"TotalAmount\", \"CreatedAt\", \"IsPaid\" FROM \"Orders\" ORDER BY \"CreatedAt\" DESC";
            
            // Dapper'ın entity yerine direkt DTO'ya map etmesini sağlıyoruz
            var orders = await dbConnection.QueryAsync<OrderDto>(query);
            
            return orders;
        }
    }
}
