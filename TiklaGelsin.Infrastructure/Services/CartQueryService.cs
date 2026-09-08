using System;
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
    public class CartQueryService : ICartQueryService
    {
        private readonly string _connectionString;

        public CartQueryService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                                ?? throw new ArgumentNullException("DefaultConnection");
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItemsAsync(Guid userId)
        {
            using IDbConnection dbConnection = new NpgsqlConnection(_connectionString);
            
            string query = @"
                SELECT 
                    c.""Id"", 
                    c.""ProductId"", 
                    p.""Name"" AS ""ProductName"", 
                    c.""Quantity"", 
                    c.""UnitPrice"",
                    (c.""Quantity"" * c.""UnitPrice"") AS ""TotalPrice""
                FROM ""CartItems"" c
                INNER JOIN ""Products"" p ON c.""ProductId"" = p.""Id""
                WHERE c.""UserId"" = @UserId";
            
            return await dbConnection.QueryAsync<CartItemDto>(query, new { UserId = userId });
        }
    }
}
