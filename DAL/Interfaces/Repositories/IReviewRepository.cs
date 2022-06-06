using DAL.Entities;

namespace DAL.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<Review> GetByIdAsync(int id);
        Task InsertAsync(Review t);
        Task UpdateAsync(Review t);
    }
}
