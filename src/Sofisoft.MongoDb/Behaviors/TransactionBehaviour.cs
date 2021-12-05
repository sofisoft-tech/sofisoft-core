using System.Diagnostics.CodeAnalysis;
using MediatR;
using Sofisoft.MongoDb.Attributes;

namespace Sofisoft.MongoDb.Behaviors
{
    [ExcludeFromCodeCoverage]
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ISofisoftMongoDbContext _context;

        public TransactionBehavior(ISofisoftMongoDbContext context)
            => _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);

            if (request.GetType().GetCustomAttributes(typeof(UseTransactionAttribute), false).Length == 0)
            {
                return await next();
            }

            if (_context.HasActiveTransaction)
            {
                return await next();
            }

            using (var session = await _context.BeginSessionAsync())
            {
                session!.StartTransaction();
                response = await next();
                await session.CommitTransactionAsync(cancellationToken);
            }

            return response;
        }
    }
}