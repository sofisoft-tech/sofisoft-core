using MongoDB.Driver;
using Sofisoft.Abstractions;
using Sofisoft.Abstractions.Attributes;
using Sofisoft.Abstractions.Repositories;
using Sofisoft.MongoDb.Models;

namespace Sofisoft.MongoDb.Repositories
{
    public class CommandRepository<TDocument> : ICommandRepository<TDocument>
        where TDocument : BaseEntity
    {
        #region Members

        private readonly ISofisoftDbContext<IClientSessionHandle> _ctx;

        #endregion

        #region Constructors

        public CommandRepository(ISofisoftDbContext<IClientSessionHandle> context)
        {
            _ctx = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region Implementations

        public virtual Task DeleteByIdAsync(
            string id,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<TDocument>
                .Filter.Eq(doc => doc.Id, id);

            if(_ctx.HasActiveTransaction)
            {
                return GetCollection().FindOneAndDeleteAsync(
                    _ctx.GetCurrentTransaction(),
                    filter,
                    cancellationToken: cancellationToken
                );
            }

            return GetCollection().FindOneAndDeleteAsync(
                filter,
                cancellationToken: cancellationToken
            );
        }

        public virtual Task InsertOneAsync(
            TDocument entity,
            CancellationToken cancellationToken = default)
        {
            if(_ctx.HasActiveTransaction)
            {
                return GetCollection()
                    .InsertOneAsync(
                        _ctx.GetCurrentTransaction(),
                        entity,
                        cancellationToken: cancellationToken);
            }

            return GetCollection()
                    .InsertOneAsync(
                        entity,
                        cancellationToken: cancellationToken);
        }

        public virtual Task InsertManyAsync(
            ICollection<TDocument> entities,
            CancellationToken cancellationToken = default)
        {
            if(_ctx.HasActiveTransaction)
            {
                return GetCollection()
                    .InsertManyAsync(
                        _ctx.GetCurrentTransaction(),
                        entities,
                        cancellationToken: cancellationToken);
            }

            return GetCollection()
                .InsertManyAsync(
                    entities,
                    cancellationToken: cancellationToken);
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
                            _ctx.GetCurrentTransaction(),
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

        #region  Publics Methods

        public IMongoCollection<TDocument> GetCollection()
        {
            return _ctx.GetDatabase<IMongoDatabase>()
                .GetCollection<TDocument>(
                    GetCollectionName(typeof(TDocument)));
        }

        #endregion

        #region  Privates Methods

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