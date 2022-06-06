using BLL.DTO.Requests;
using BLL.DTO.Responses;

namespace BLL.Interfaces.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewResponse>> GetAsync();

        //ToDo
        //Task<PagedList<ReviewResponse>> GetAsync(ReviewParameters parameters);

        Task<ReviewResponse> GetByIdAsync(int id);

        Task InsertAsync(ReviewRequest request);

        Task UpdateAsync(ReviewRequest request);

        Task DeleteAsync(int id);
    }
}
