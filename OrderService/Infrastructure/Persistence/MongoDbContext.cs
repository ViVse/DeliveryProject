using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using MongoDB.Driver;

namespace Infrastructure.Persistence
{
    public class MongoDbContext: IMongoDbContext
    {
        private readonly IMediator _mediator;
        private readonly string _connectionString;
        private readonly string _dbName;

        public MongoDbContext(IMediator mediator, string connectionString, string dbName)
        {
            _mediator = mediator;
            _connectionString = connectionString;
            _dbName = dbName;
        }

        public IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(_connectionString);
            var db = client.GetDatabase(_dbName);
            return db.GetCollection<T>(collection);
        }

        public async Task PublishEvents(IEnumerable<BaseEvent> events)
        {
            foreach(var domainEvent in events) 
                await _mediator.Publish(domainEvent);
        }
    }
}
