using System.ComponentModel;
using Sofisoft.Identity;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Exposes the necessary methods required to configure the Sofisoft Identity services.
    /// </summary>
    public class SofisoftIdentityBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SofisoftIdentityBuilder"/>.
        /// </summary>
        /// <param name="services">The services collection.</param>
        public SofisoftIdentityBuilder(IServiceCollection services)
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
        /// <returns>The <see cref="SofisoftIdentityBuilder"/>.</returns>
        public SofisoftIdentityBuilder Configure(Action<SofisoftIdentityOptions> configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            Services.Configure(configuration);

            return this;
        }

        /// <summary>
        /// Sets the key for the ClientId.
        /// </summary>
        /// <param name="key">The services uri.</param>
        /// <returns>The <see cref="SofisoftIdentityBuilder"/>.</returns>
        public SofisoftIdentityBuilder SetClientIdKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Configure(options => options.ClientIdKey = key);
        }

        /// <summary>
        /// Sets the key for the CompanyId.
        /// </summary>
        /// <param name="key">The services uri.</param>
        /// <returns>The <see cref="SofisoftIdentityBuilder"/>.</returns>
        public SofisoftIdentityBuilder SetCompanyIdKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Configure(options => options.CompanyIdKey = key);
        }

        /// <summary>
        /// Sets the key for the UserId.
        /// </summary>
        /// <param name="key">The services uri.</param>
        /// <returns>The <see cref="SofisoftIdentityBuilder"/>.</returns>
        public SofisoftIdentityBuilder SetUserIdKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Configure(options => options.UserIdKey = key);
        }

        /// <summary>
        /// Sets the key for the UserName.
        /// </summary>
        /// <param name="key">The services uri.</param>
        /// <returns>The <see cref="SofisoftIdentityBuilder"/>.</returns>
        public SofisoftIdentityBuilder SetUsernameKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Configure(options => options.UsernameKey = key);
        }
        
    }
}