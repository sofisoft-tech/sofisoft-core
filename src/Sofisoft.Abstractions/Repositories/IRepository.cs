using System.Linq.Expressions;
using Sofisoft.Abstractions.Models;

namespace Sofisoft.Abstractions.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : IEntity
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
        /// Update a single entity.
        /// </summary>
        /// <param name="entity">Entity being modified.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        Task<long> UpdateOneAsync(
            TEntity entity,
            CancellationToken cancellationToken = default);
    }
}