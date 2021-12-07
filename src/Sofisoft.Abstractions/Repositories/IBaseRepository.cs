using System.Linq.Expressions;
using Sofisoft.Abstractions.Models;

namespace Sofisoft.Abstractions.Repositories
{
    public interface IBaseRepository<TEntity>
        where TEntity : IBaseEntity
    {
        /// <summary>
        /// Returns number of records according to the expression.
        /// </summary>
        /// <param name="filterExpression">Expression.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        Task<long> CountAsync(
            Expression<Func<TEntity, bool>> filterExpression,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete a record from the database by Id.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        Task DeleteByIdAsync(
            string id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Filter entities by an expression.
        /// </summary>
        /// <param name="filterExpression">Expression to filter.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        Task<IEnumerable<TEntity>> FilterByAsync(
            Expression<Func<TEntity, bool>> filterExpression,
            CancellationToken cancellationToken = default);

        // <summary>
        /// Filters entities by an expression and returns projection.
        /// </summary>
        /// <param name="filterExpression">Expression to filter.</param>
        /// <param name="projectionExpression">Projection expression.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<IEnumerable<TProjected>> FilterByAsync<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Filter an entity by Id.
        /// </summary>
        /// <param name="id">Id of the entity to be filtered.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<TEntity> FindByIdAsync(
            string id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Search for an entity by an expression.
        /// </summary>
        /// <param name="filterExpression">Expression to filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<TEntity> FindOneAsync(
            Expression<Func<TEntity, bool>> filterExpression,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Search for an entity by an expression.
        /// </summary>
        /// <param name="filterExpression">Expression to filter.</param>
        /// <param name="projectionExpression">Projection expression.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<TProjected> FindOneAsync<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Insert many entities.
        /// </summary>
        /// <param name="entities">Collection of entities to be inserted.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        Task InsertManyAsync(
            ICollection<TEntity> entities,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns paged list.
        /// </summary>
        /// <param name="filterExpression">Expression to filter.</param>
        /// <param name="sort">Ordering criteria.</param>
        /// <param name="pageSize">Number of records returned.</param>
        /// <param name="start">Registration number from.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        Task<IEnumerable<TEntity>> PaginatedAsync(
            Expression<Func<TEntity, bool>> filterExpression,
            string sort,
            int pageSize,
            int start,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns paged list.
        /// </summary>
        /// <param name="filterExpression">Expression to filter.</param>
        /// <param name="projectionExpression">Projection expression.</param>
        /// <param name="sort">Ordering criteria.</param>
        /// <param name="pageSize">Number of records returned.</param>
        /// <param name="start">Registration number from.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        Task<IEnumerable<TProjected>> PaginatedAsync<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression,
            string sort,
            int pageSize,
            int start,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Update a single entity.
        /// </summary>
        /// <param name="entity">Entity being modified.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        Task<long> UpdateOneAsync(
            TEntity entity,
            CancellationToken cancellationToken = default);

    }
}