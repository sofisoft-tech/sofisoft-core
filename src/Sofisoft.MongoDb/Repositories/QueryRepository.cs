using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using MongoDB.Driver;
using Sofisoft.Abstractions;
using Sofisoft.Abstractions.Attributes;
using Sofisoft.Abstractions.Repositories;
using Sofisoft.MongoDb.Models;

namespace Sofisoft.MongoDb.Repositories
{
    public class QueryRepository<TDocument> : IQueryRepository<TDocument>
        where TDocument : BaseEntity
    {
        #region Members

        private readonly ISofisoftDbContext<IClientSessionHandle> _ctx;

        #endregion

        #region Constructors

        public QueryRepository(ISofisoftDbContext<IClientSessionHandle> context)
        {
            _ctx = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region Implementations

        public virtual Task<long> CountAsync(
            Expression<Func<TDocument, bool>> filterExpression,
            CancellationToken cancellationToken = default)
        {
            if (_ctx.HasActiveTransaction)
            {
                return GetCollection().CountDocumentsAsync(
                    _ctx.GetCurrentTransaction(),
                    filterExpression ?? FilterDefinition<TDocument>.Empty,
                    cancellationToken: cancellationToken);
            }

            return GetCollection().CountDocumentsAsync(
                filterExpression ?? FilterDefinition<TDocument>.Empty,
                cancellationToken: cancellationToken
            );
        }

        public virtual Task<IEnumerable<TDocument>> FilterByAsync(
            Expression<Func<TDocument, bool>> filterExpression,
            CancellationToken cancellationToken = default)
        {
            if(_ctx.HasActiveTransaction)
            {
                return Task.Run(() => 
                    GetCollection().Find(
                        _ctx.GetCurrentTransaction(),
                        filterExpression ?? FilterDefinition<TDocument>.Empty)
                        .ToEnumerable(cancellationToken)
                );
            }

            return Task.Run(() => 
                GetCollection().Find(
                    filterExpression ?? FilterDefinition<TDocument>.Empty)
                    .ToEnumerable(cancellationToken)
            );
        }

        [ExcludeFromCodeCoverage]
        public virtual Task<IEnumerable<TProjected>> FilterByAsync<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression,
            CancellationToken cancellationToken = default)
        {
            if(_ctx.HasActiveTransaction)
            {
                return Task.Run(() =>
                    GetCollection().Find(
                        _ctx.GetCurrentTransaction(),
                        filterExpression ?? FilterDefinition<TDocument>.Empty)
                        .Project(projectionExpression)
                        .ToEnumerable(cancellationToken)
                );
            }

            return Task.Run(() =>
                GetCollection().Find(
                    filterExpression ?? FilterDefinition<TDocument>.Empty)
                    .Project(projectionExpression)
                    .ToEnumerable(cancellationToken)
            );
        }

        [ExcludeFromCodeCoverage]
        public virtual Task<TDocument> FindByIdAsync(
            string id,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);

            if(_ctx.HasActiveTransaction)
            {
                return GetCollection()
                    .Find(
                        _ctx.GetCurrentTransaction(),
                        filter)
                    .SingleOrDefaultAsync(cancellationToken);
            }

            return GetCollection()
                .Find(filter)
                .SingleOrDefaultAsync(cancellationToken);
        }

        [ExcludeFromCodeCoverage]
        public virtual Task<TDocument> FindOneAsync(
            Expression<Func<TDocument, bool>> filterExpression,
            CancellationToken cancellationToken = default)
        {
            if(_ctx.HasActiveTransaction)
            {
                return GetCollection().Find(
                    _ctx.GetCurrentTransaction(),
                    filterExpression ?? FilterDefinition<TDocument>.Empty)
                    .Limit(1)
                    .SingleOrDefaultAsync(cancellationToken);
            }

            return Task.Run(() =>
                GetCollection().Find(
                    filterExpression ?? FilterDefinition<TDocument>.Empty)
                    .Limit(1)
                    .SingleOrDefaultAsync(cancellationToken)
            );
        }

        [ExcludeFromCodeCoverage]
        public virtual Task<TProjected> FindOneAsync<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression,
            CancellationToken cancellationToken = default)
        {
            if(_ctx.HasActiveTransaction)
            {
                return GetCollection().Find(
                        _ctx.GetCurrentTransaction(),
                        filterExpression ?? FilterDefinition<TDocument>.Empty)
                    .Project(projectionExpression)
                    .Limit(1)
                    .SingleOrDefaultAsync(cancellationToken);
            }

            return GetCollection().Find(
                    filterExpression ?? FilterDefinition<TDocument>.Empty)
                .Project(projectionExpression)
                .Limit(1)
                .SingleOrDefaultAsync(cancellationToken);
        }

        [ExcludeFromCodeCoverage]
        public virtual Task<IEnumerable<TDocument>> PaginatedAsync(
            Expression<Func<TDocument, bool>> filterExpression,
            string sort,
            int pageSize,
            int start,
            CancellationToken cancellationToken = default)
        {
            if(_ctx.HasActiveTransaction)
            {
                return Task.Run(() => 
                    GetCollection().Find(
                            _ctx.GetCurrentTransaction(),
                            filterExpression ?? FilterDefinition<TDocument>.Empty)
                        .Sort(sort)
                        .Skip(start)
                        .Limit(pageSize)
                        .ToEnumerable(cancellationToken)
                );
            }

            return Task.Run(() => 
                GetCollection().Find(
                        filterExpression ?? FilterDefinition<TDocument>.Empty)
                    .Sort(sort)
                    .Skip(start)
                    .Limit(pageSize)
                    .ToEnumerable(cancellationToken)
            );
        }

        [ExcludeFromCodeCoverage]
        public virtual Task<IEnumerable<TProjected>> PaginatedAsync<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression,
            string sort,
            int pageSize,
            int start,
            CancellationToken cancellationToken = default)
        {
            if(_ctx.HasActiveTransaction)
            {
                return Task.Run(() => 
                    GetCollection().Find(
                            _ctx.GetCurrentTransaction(),
                            filterExpression ?? FilterDefinition<TDocument>.Empty)
                        .Project(projectionExpression)
                        .Sort(sort)
                        .Skip(start)
                        .Limit(pageSize)
                        .ToEnumerable(cancellationToken)
                );
            }

            return Task.Run(() => 
                GetCollection().Find(
                        filterExpression ?? FilterDefinition<TDocument>.Empty)
                    .Project(projectionExpression)
                    .Sort(sort)
                    .Skip(start)
                    .Limit(pageSize)
                    .ToEnumerable(cancellationToken)
            );
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

        #endregion

    }
}