using System.Linq.Expressions;
using MongoDB.Driver;
using Sofisoft.Abstractions.Models;
using Sofisoft.MongoDb.Attributes;

namespace Sofisoft.MongoDb.Repositories
{
    public class MongoDbRepository<TDocument> : IMongoDbRepository<TDocument>
        where TDocument : IEntity
    {
        #region Members

        private readonly ISofisoftMongoDbContext _ctx;

        #endregion

        #region Constructors

        public MongoDbRepository(ISofisoftMongoDbContext context)
        {
            _ctx = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region Implementations

        public virtual Task<IEnumerable<TResult>> AggregateAsync<TResult>(
            PipelineDefinition<TDocument, TResult> pipeline,
            CancellationToken cancellationToken = default)
        {
            if(_ctx.HasActiveTransaction)
            {
                return Task.Run(() =>
                    GetCollection()
                        .Aggregate(
                            _ctx.GetCurrentSession(),
                            pipeline,
                            cancellationToken: cancellationToken)
                        .ToEnumerable()
                );
            }
            
            return Task.Run(() =>
                GetCollection()
                    .Aggregate(
                        pipeline,
                        cancellationToken: cancellationToken)
                    .ToEnumerable()
            );
        }

        public virtual Task<long> CountAsync(
            Expression<Func<TDocument, bool>> filterExpression,
            CancellationToken cancellationToken = default)
        {
            if (_ctx.HasActiveTransaction)
            {
                return GetCollection().CountDocumentsAsync(
                    _ctx.GetCurrentSession(),
                    filterExpression ?? FilterDefinition<TDocument>.Empty,
                    cancellationToken: cancellationToken);
            }

            return GetCollection().CountDocumentsAsync(
                filterExpression ?? FilterDefinition<TDocument>.Empty,
                cancellationToken: cancellationToken
            );
        }

        #endregion

        #region  Privates Methods

        private IMongoCollection<TDocument> GetCollection()
        {
            return _ctx.Database
                .GetCollection<TDocument>(
                    GetCollectionName(typeof(TDocument)));
        }

        private static string GetCollectionName(Type documentType)
        {
            var attribute = (BsonCollectionAttribute?) documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault();

            if (attribute is null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.CollectionName;
        }

        private static UpdateDefinition<TDocument> GetUpdateDefinition(TDocument document)
        {
            var builder = new UpdateDefinitionBuilder<TDocument>();
            var updates = document.GetType().GetProperties()
                    .Where(x => !new string[] { "Id", "CreatedAt", "CreatedBy" }.Contains(x.Name)
                        && (x.GetValue(document) is not null || x.Name == "ModifiedAt"))
                    .Select(x => x.Name == "ModifiedAt"
                        ? builder.Set(x.Name, DateTime.UtcNow)
                        : builder.Set(x.Name, x.GetValue(document)));
            
            return builder.Combine(updates);
        }

        #endregion

    }
}