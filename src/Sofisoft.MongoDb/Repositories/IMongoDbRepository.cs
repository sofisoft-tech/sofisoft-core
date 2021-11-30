using MongoDB.Driver;
using Sofisoft.Abstractions.Models;
using Sofisoft.Abstractions.Repositories;

namespace Sofisoft.MongoDb.Repositories
{
    public interface IMongoDbRepository<TDocument> : IRepository<TDocument>
        where TDocument : IEntity
    {

        Task<IEnumerable<TResult>> AggregateAsync<TResult>(
            PipelineDefinition<TDocument, TResult> pipeline,
            CancellationToken cancellationToken = default);

    }

}