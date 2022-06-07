using MongoDB.Driver;

namespace Application.Common.Interfaces
{
    public interface IMongoDbContext
    {
        public IMongoCollection<T> ConnectToMongo<T>(in string collection);

        Task<string> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
