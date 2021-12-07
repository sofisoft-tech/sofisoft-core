using Microsoft.Extensions.DependencyInjection.Extensions;
using Sofisoft.Abstractions.Managers;
using Sofisoft.Logging;
using Sofisoft.Logging.Managers;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Exposes extensions allowing to register the Sofisoft logging services.
    /// </summary>
    public static class SofisoftLoggingExtensions
    {
        /// <summary>
        /// Registers the Sofisoft logging services in the DI container.
        /// </summary>
        /// <param name="builder">The services builder used by Sofisoft to register new services.</param>
        /// <remarks>This extension can be safely called multiple times.</remarks>
        /// <returns>The <see cref="SofisoftBuilder"/>.</returns>
        public static SofisoftLoggingBuilder AddLogging(this SofisoftBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddScoped<ILoggerManager, LoggerManager>();

            return new SofisoftLoggingBuilder(builder.Services);
        }

        /// <summary>
        /// Registers the Sofisoft logging services in the DI container.
        /// </summary>
        /// <param name="builder">The services builder used by Sofisoft to register new services.</param>
        /// <param name="configuration">The configuration delegate used to configure the core services.</param>
        /// <remarks>This extension can be safely called multiple times.</remarks>
        /// <returns>The <see cref="SofisoftBuilder"/>.</returns>
        public static SofisoftBuilder AddLogging(this SofisoftBuilder builder, Action<SofisoftLoggingBuilder> configuration)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            configuration(builder.AddLogging());

            return builder;
        }
    }

}