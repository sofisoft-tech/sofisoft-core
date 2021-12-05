using Microsoft.Extensions.DependencyInjection.Extensions;
using Sofisoft.MongoDb;
using Sofisoft.MongoDb.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Exposes extensions allowing to register the Sofisoft mongoDb services.
    /// </summary>
    public static class SofisoftMongoDbExtensions
    {
        /// <summary>
        /// Registers the Sofisoft mongoDb services in the DI container.
        /// </summary>
        /// <param name="builder">The services builder used by Sofisoft to register new services.</param>
        /// <remarks>This extension can be safely called multiple times.</remarks>
        /// <returns>The <see cref="SofisoftBuilder"/>.</returns>
        public static SofisoftMongoDbBuilder AddMongoDb(this SofisoftBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            builder.Services.TryAddScoped(typeof(ISofisoftMongoDbContext), typeof(SofisoftMongoDbContext));

            return new SofisoftMongoDbBuilder(builder.Services);
        }

        /// <summary>
        /// Registers the Sofisoft mongoDb services in the DI container.
        /// </summary>
        /// <param name="builder">The services builder used by Sofisoft to register new services.</param>
        /// <param name="configuration">The configuration delegate used to configure the core services.</param>
        /// <remarks>This extension can be safely called multiple times.</remarks>
        /// <returns>The <see cref="SofisoftBuilder"/>.</returns>
        public static SofisoftBuilder AddMongoDb(this SofisoftBuilder builder, Action<SofisoftMongoDbBuilder> configuration)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            configuration(builder.AddMongoDb());

            return builder;
        }
    }

}