using DAL.Pagination;
using DAL.Parameters;
using System.Linq.Expressions;

namespace DAL.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAsync();
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression);
        Task<PagedList<TEntity>> GetAsync(PaginationParameters parameters);
        Task<TEntity> GetByIdAsync(int id);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
    }
}
