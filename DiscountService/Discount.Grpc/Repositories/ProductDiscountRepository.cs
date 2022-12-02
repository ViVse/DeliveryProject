using Dapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Repositories.Interfaces;
using Npgsql;

namespace Discount.Grpc.Repositories
{
    public class ProductDiscountRepository: IProductDiscountRepository
    { 
        private readonly IConfiguration _configuration;

        public ProductDiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<ProductDiscount> GetDiscount(int id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var discount = await connection.QueryFirstOrDefaultAsync<ProductDiscount>("SELECT * FROM ProductDiscount WHERE ProductId = @ProductId", new { ProductId = id });

            if (discount is null)
            {
                return new ProductDiscount { ProductId = -1, Amount = 0, IsPercent = false };
            }

            return discount;
        }

        public async Task<bool> CreateDiscount(ProductDiscount discount)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("INSERT INTO ProductDiscount (ProductId, Amount, IsPercent) VALUES (@ProductId, @Amount, @IsPercent)",
                new { ProductId = discount.ProductId, Amount = discount.Amount, IsPercent = discount.IsPercent });

            return affected != 0;
        }

        public async Task<bool> DeleteDiscount(int id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM ProductDiscount WHERE Id = @Id", new { Id = id });

            return affected != 0;
        }

        public async Task<bool> UpdateDiscount(ProductDiscount discount)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("UPDATE Coupon SET ProductId=@ProductId, Amount=@Amount, IsPercent=@IsPercent WHERE Id=@Id",
                new { ProductId = discount.ProductId, Amount = discount.Amount, IsPercent = discount.IsPercent, Id = discount.Id });

            return affected != 0;
        }
    }
}
