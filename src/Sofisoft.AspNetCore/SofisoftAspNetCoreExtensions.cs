using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Sofisoft.AspNetCore;
using Sofisoft.AspNetCore.Middlewares;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Exposes extensions allowing to register the Sofisoft aspnetcore services.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SofisoftAspNetCoreExtensions
    {
        /// <summary>
        /// Register the Logging middleware.
        /// </summary>
        public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
            => app.UseMiddleware<LoggingMiddleware>();

        /// <summary>
        /// Register the Transaction middleware.
        /// </summary>
        public static IApplicationBuilder UseDbTransaction<TContextTransaction>(this IApplicationBuilder app)
           where TContextTransaction : class, IDisposable
           => app.UseMiddleware<TransactionMiddleware<TContextTransaction>>();
    }
}