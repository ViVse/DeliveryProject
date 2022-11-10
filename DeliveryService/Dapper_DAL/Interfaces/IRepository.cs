using Dapper_DAL.Pagination;
using Dapper_DAL.Parameters;

namespace Dapper_DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAsync();
        Task<PagedList<TEntity>> GetAsync(PaginationParameters parameters);
        Task<TEntity> GetByIdAsync(int id);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
    }
}
