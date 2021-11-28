using Microsoft.Extensions.DependencyInjection.Extensions;
using Sofisoft.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Exposes extensions allowing to register the Sofisoft services.
    /// </summary>
    public static class SofisoftExtensions
    {
        /// <summary>
        /// Provides a common entry point for registering the Sofisoft services.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <remarks>This extension can be safely called multiple times.</remarks>
        /// <returns>The <see cref="SofisoftBuilder"/>.</returns>
        public static SofisoftBuilder AddSofisoft(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddScoped<SofisoftConfiguration>();

            return new SofisoftBuilder(services);
        }

        /// <summary>
        /// Provides a common entry point for registering the Sofisoft services.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <param name="configuration">The configuration delegate used to register new services.</param>
        /// <remarks>This extension can be safely called multiple times.</remarks>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddSofisoft(this IServiceCollection services, Action<SofisoftBuilder> configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            configuration(services.AddSofisoft());

            return services;
        }
    }
}