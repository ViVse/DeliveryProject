using Domain.Common;
using MongoDB.Driver;

namespace Application.Common.Interfaces
{
    public interface IMongoDbContext
    {
        public IMongoCollection<T> ConnectToMongo<T>(in string collection);

        public Task PublishEvents(IEnumerable<BaseEvent> events);
    }
}
