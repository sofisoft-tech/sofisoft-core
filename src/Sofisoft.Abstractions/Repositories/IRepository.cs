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
        /// <param name="filterExpression">Expression</param>
        /// <param name="cancellationToken">cancellation token</param>
        Task<long> CountAsync(
            Expression<Func<TEntity, bool>> filterExpression,
            CancellationToken cancellationToken = default);
    }
}