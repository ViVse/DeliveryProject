using DAL.Entities;

namespace DAL.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAll();
        Task Delete(int id);
        Task<Review> GetById(int id);
        Task Insert(Review t);
        Task Update(Review t);
    }
}
