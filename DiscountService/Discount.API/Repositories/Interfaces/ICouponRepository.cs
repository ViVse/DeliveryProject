using Discount.API.Entities;

namespace Discount.API.Repositories
{
    public interface ICouponRepository
    {
        Task<Coupon> GetCoupon(string code);

        Task<bool> CreateCoupon(Coupon coupon);

        Task<bool> UpdateCoupon(Coupon coupon);

        Task<bool> DeleteCoupon(int id);
    }
}
