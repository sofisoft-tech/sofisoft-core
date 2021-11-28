using System.ComponentModel;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides a shared entry point allowing to configure the Sofisoft services.
    /// </summary>
    public class SofisoftBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SofisoftBuilder"/>.
        /// </summary>
        /// <param name="services">The services collection.</param>
        public SofisoftBuilder(IServiceCollection services)
            => Services = services ?? throw new ArgumentNullException(nameof(services));

        /// <summary>
        /// Gets the services collection.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IServiceCollection Services { get; }

    }
}