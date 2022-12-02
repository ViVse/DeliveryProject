using Discount.Grpc.Entities;

namespace Discount.Grpc.Repositories.Interfaces
{
    public interface IProductDiscountRepository
    {
        Task<ProductDiscount> GetDiscount(int id);

        Task<bool> CreateDiscount(ProductDiscount coupon);

        Task<bool> UpdateDiscount(ProductDiscount coupon);

        Task<bool> DeleteDiscount(int id);
    }
}
