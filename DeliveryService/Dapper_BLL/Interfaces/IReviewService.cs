using Dapper_BLL.DTO.Requests;
using Dapper_BLL.DTO.Responses;

namespace Dapper_BLL.Interfaces
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
