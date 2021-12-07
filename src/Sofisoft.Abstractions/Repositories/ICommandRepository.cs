using Sofisoft.Abstractions.Models;

namespace Sofisoft.Abstractions.Repositories
{
    public interface ICommandRepository<TEntity>
        where TEntity : IBaseEntity
    {

        /// <summary>
        /// Delete a record from the database by Id.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        Task DeleteByIdAsync(
            string id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Insert a single entity
        /// </summary>
        /// <param name="entity">Entity to be inserted.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task InsertOneAsync(
            TEntity entity,
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
        /// Update a single entity.
        /// </summary>
        /// <param name="entity">Entity being modified.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        Task<long> UpdateOneAsync(
            TEntity entity,
            CancellationToken cancellationToken = default);
    }
}