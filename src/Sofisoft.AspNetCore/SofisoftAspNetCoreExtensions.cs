using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Sofisoft.AspNetCore.Middlewares;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Exposes extensions allowing to register the Sofisoft aspnetcore services.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SofisoftAspNetCoreExtensions
    {
        public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
            => app.UseMiddleware<LoggingMiddleware>();

        public static IApplicationBuilder UseDbTransaction<TContextTransaction>(this IApplicationBuilder app)
           where TContextTransaction : class, IDisposable
           => app.UseMiddleware<TransactionMiddleware<TContextTransaction>>();
    }
}