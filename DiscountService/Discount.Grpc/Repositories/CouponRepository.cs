using Dapper;
using Discount.Grpc.Entities;
using Npgsql;

namespace Discount.Grpc.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly IConfiguration _configuration;

        public CouponRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Coupon> GetCoupon(string code)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE Code = @Code", new { Code = code });

            if(coupon is null)
            {
                return new Coupon { Code = "No_discount", Amount = 0, IsPercent = false, Description = "No Desc" };
            }

            return coupon;
        }

        public async Task<bool> CreateCoupon(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("INSERT INTO Coupon (Code, Description, Amount, IsPercent) VALUES (@Code, @Description, @Amount, @IsPercent)", 
                new { Code = coupon.Code, Description = coupon.Description, Amount = coupon.Amount, IsPercent = coupon.IsPercent });

            return affected != 0;
        }

        public async Task<bool> DeleteCoupon(int id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE Id = @Id", new { Id = id });

            return affected != 0;
        }

        public async Task<bool> UpdateCoupon(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("UPDATE Coupon SET Code=@Code, Description=@Description, Amount=@Amount, IsPercent=@IsPercent WHERE Id=@Id",
                new {Code = coupon.Code, Description = coupon.Description, Amount = coupon.Amount, IsPercent = coupon.IsPercent, Id = coupon.Id});

            return affected != 0;
        }
    }
}
