using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Sofisoft.Abstractions;
using Sofisoft.Abstractions.Attributes;

namespace Sofisoft.AspNetCore.Middlewares
{
    [ExcludeFromCodeCoverage]
    public sealed class TransactionMiddleware<TContextTransaction> : IMiddleware
        where TContextTransaction : class, IDisposable
    {
        private readonly ISofisoftDbContext<TContextTransaction> _context;

        public TransactionMiddleware(ISofisoftDbContext<TContextTransaction> context)
            => _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Method.Equals("GET", StringComparison.CurrentCultureIgnoreCase))
            {
                await next(context);
                return;
            }

            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<TransactionAttribute>();

            if (attribute is null)
            {
                await next(context);
                return;
            }

            using (var transaction = await _context.BeginTransactionAsync())
            {
                 await next(context);
                 await _context.CommitTransactionAsync(transaction);
            }
        }
    }

}