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

        public virtual Task DeleteByIdAsync(
            string id,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<TDocument>
                .Filter.Eq(doc => doc.Id, id);

            if(_ctx.HasActiveTransaction)
            {
                return GetCollection().FindOneAndDeleteAsync(
                    _ctx.GetCurrentSession(),
                    filter,
                    cancellationToken: cancellationToken
                );
            }

            return GetCollection().FindOneAndDeleteAsync(
                filter,
                cancellationToken: cancellationToken
            );
        }

        public virtual Task<long> UpdateOneAsync(
            TDocument entity,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, entity.Id);
            var update = GetUpdateDefinition(entity);
            var options = new UpdateOptions { IsUpsert = false };

            if(_ctx.HasActiveTransaction)
            {
                return Task.Run(() =>
                {
                    var updateResult = GetCollection()
                        .UpdateOne(
                            _ctx.GetCurrentSession(),
                            filter,
                            update,
                            options, cancellationToken);

                    return updateResult.ModifiedCount;
                }, cancellationToken);
            }

            return Task.Run(() =>
            {
                var updateResult = GetCollection()
                    .UpdateOne(
                        filter,
                        update,
                        options,
                        cancellationToken);

                    return updateResult.ModifiedCount;
                }, cancellationToken);
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
                throw new KeyNotFoundException(nameof(attribute));
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