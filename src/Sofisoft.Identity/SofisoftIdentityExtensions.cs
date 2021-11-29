using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sofisoft.Abstractions.Managers;
using Sofisoft.Identity.Managers;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Exposes extensions allowing to register the Sofisoft identity services.
    /// </summary>
    public static class SofisoftIdentityExtensions
    {
        /// <summary>
        /// Registers the Sofisoft identity services in the DI container.
        /// </summary>
        /// <param name="builder">The services builder used by Sofisoft to register new services.</param>
        /// <remarks>This extension can be safely called multiple times.</remarks>
        /// <returns>The <see cref="SofisoftIdentityBuilder"/>.</returns>
        public static SofisoftIdentityBuilder AddIdentity(this SofisoftBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddScoped<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.TryAddScoped<IIdentityManager, IdentityManager>();

            return new SofisoftIdentityBuilder(builder.Services);
        }

        /// <summary>
        /// Registers the Sofisoft identity services in the DI container.
        /// </summary>
        /// <param name="builder">The services builder used by Sofisoft to register new services.</param>
        /// <param name="configuration">The configuration delegate used to configure the core services.</param>
        /// <remarks>This extension can be safely called multiple times.</remarks>
        /// <returns>The <see cref="SofisoftBuilder"/>.</returns>
        public static SofisoftBuilder AddIdentity(this SofisoftBuilder builder, Action<SofisoftIdentityBuilder> configuration)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            configuration(builder.AddIdentity());

            return builder;
        }
    }

}