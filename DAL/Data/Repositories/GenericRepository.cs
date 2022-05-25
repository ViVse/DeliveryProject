using DAL.Exceptions;
using DAL.Interfaces.Repositories;
using DAL.Pagination;
using DAL.Parameters;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Data.Repositories
{
    public class GenericRepository<TEntity>: IRepository<TEntity> where TEntity : class
    {
        protected readonly Context databaseContext;

        protected readonly DbSet<TEntity> table;

        public GenericRepository(Context dbContext)
        {
            databaseContext = dbContext;
            table = databaseContext.Set<TEntity>();
        }

        protected static string GetEntityNotFoundErrorMeassage(int id) => $"{typeof(TEntity).Name} with id {id} was not found.";

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return table.Where(expression);
        }
        
        public virtual async Task<IEnumerable<TEntity>> GetAsync() => await table.ToListAsync();

        public async Task<PagedList<TEntity>> GetAsync(PaginationParameters parameters)
        {
            IQueryable<TEntity> source = table;

            return await PagedList<TEntity>.ToPagedListAsync(source, parameters.PageNumber, parameters.PageSize);
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await table.FindAsync(id) ?? throw new EntityNotFoundException(GetEntityNotFoundErrorMeassage(id));
        }

        public virtual async Task InsertAsync(TEntity entity) => await Task.Run(() => table.AddAsync(entity));

        public virtual async Task UpdateAsync(TEntity entity) => await Task.Run(() => table.Update(entity));

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            await Task.Run(() => table.Remove(entity));
        }
    }
}
