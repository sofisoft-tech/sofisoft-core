using System.ComponentModel;
using Sofisoft.MongoDb;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Exposes the necessary methods required to configure the Sofisoft MongoDb.
    /// </summary>
    public class SofisoftMongoDbBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SofisoftMongoDbBuilder"/>.
        /// </summary>
        /// <param name="services">The services collection.</param>
        public SofisoftMongoDbBuilder(IServiceCollection services)
            => Services = services ?? throw new ArgumentNullException(nameof(services));

        /// <summary>
        /// Gets the services collection.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IServiceCollection Services { get; }

        /// <summary>
        /// Amends the default Sofisoft validation configuration.
        /// </summary>
        /// <param name="configuration">The delegate used to configure the Sofisoft options.</param>
        /// <remarks>This extension can be safely called multiple times.</remarks>
        /// <returns>The <see cref="SofisoftLoggingBuilder"/>.</returns>
        public SofisoftMongoDbBuilder Configure(Action<SofisoftMongoDbOptions> configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            Services.Configure(configuration);

            return this;
        }

        /// <summary>
        /// Sets the connection string.
        /// </summary>
        /// <param name="connectionString">The services uri.</param>
        /// <returns>The <see cref="SofisoftLoggingBuilder"/>.</returns>
        public SofisoftMongoDbBuilder SetConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            return Configure(options => options.ConnectionString = connectionString);
        }

        /// <summary>
        /// Sets the name of the database.
        /// </summary>
        /// <param name="connectionString">The services uri.</param>
        /// <returns>The <see cref="SofisoftLoggingBuilder"/>.</returns>
        public SofisoftMongoDbBuilder SetDatabase(string database)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException(nameof(database));
            }

            return Configure(options => options.Database = database);
        }
        
    }
}