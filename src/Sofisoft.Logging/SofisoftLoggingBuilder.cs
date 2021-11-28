using System.ComponentModel;
using Sofisoft.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Exposes the necessary methods required to configure the Sofisoft logging services.
    /// </summary>
    public class SofisoftLoggingBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SofisoftLoggingBuilder"/>.
        /// </summary>
        /// <param name="services">The services collection.</param>
        public SofisoftLoggingBuilder(IServiceCollection services)
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
        public SofisoftLoggingBuilder Configure(Action<SofisoftLoggingOptions> configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            Services.Configure(configuration);

            return this;
        }

        /// <summary>
        /// Sets the base address of service.
        /// </summary>
        /// <param name="baseAddress">The services uri.</param>
        /// <returns>The <see cref="SofisoftLoggingBuilder"/>.</returns>
        public SofisoftLoggingBuilder SetBaseAddress(Uri baseAddress)
        {
            if (baseAddress is null)
            {
                throw new ArgumentNullException(nameof(baseAddress));
            }

            return Configure(options => options.BaseAddress = baseAddress);
        }

        /// <summary>
        /// Sets the base address of service.
        /// </summary>
        /// <param name="baseAddress">The services uri.</param>
        /// <returns>The <see cref="SofisoftLoggingBuilder"/>.</returns>
        public SofisoftLoggingBuilder SetBaseAddress(string baseAddress)
        {
            if (string.IsNullOrEmpty(baseAddress))
            {
                throw new ArgumentNullException(nameof(baseAddress));
            }

            if (!Uri.TryCreate(baseAddress, UriKind.Absolute, out Uri? uri) || !uri.IsWellFormedOriginalString())
            {
                throw new ArgumentException(String.Format("El argumento {0} no es válido", baseAddress), nameof(baseAddress));
            }

            return SetBaseAddress(uri);
        }

        /// <summary>
        /// Sets the end point to log an event of type error.
        /// </summary>
        /// <param name="endpoint">The path of service.</param>
        /// <returns>The <see cref="SofisoftLoggingBuilder"/>.</returns>
        public SofisoftLoggingBuilder SetErrorEndPointUri(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            return Configure(options => options.ErrorEndPointUri = endpoint);
        }

        /// <summary>
        /// Sets the end point to log an event of type information.
        /// </summary>
        /// <param name="endpoint">The path of service.</param>
        /// <returns>The <see cref="SofisoftLoggingBuilder"/>.</returns>
        public SofisoftLoggingBuilder SetInformationEndPointUri(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            return Configure(options => options.InformationEndPointUri = endpoint);
        }

        /// <summary>
        /// Set the source of the event.
        /// </summary>
        /// <param name="source">The source of event.</param>
        /// <returns>The <see cref="SofisoftLoggingBuilder"/>.</returns>
        public SofisoftLoggingBuilder SetSource(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Configure(options => options.Source = source);
        }

        /// <summary>
        /// Sets the token type for the athentication in the service.
        /// </summary>
        /// <param name="type">The token type.</param>
        /// <returns>The <see cref="SofisoftLoggingBuilder"/>.</returns>
        public SofisoftLoggingBuilder SetTokenType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            return Configure(options => options.TokenType = type);
        }

        /// <summary>
        /// Sets the token for the athentication in the service.
        /// </summary>
        /// <param name="value">The token.</param>
        /// <returns>The <see cref="SofisoftLoggingBuilder"/>.</returns>
        public SofisoftLoggingBuilder SetTokenValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return Configure(options => options.TokenValue = value);
        }

        /// <summary>
        /// Sets the end point to log an event of type warning.
        /// </summary>
        /// <param name="endpoint">The path of service.</param>
        /// <returns>The <see cref="SofisoftLoggingBuilder"/>.</returns>
        public SofisoftLoggingBuilder SetWarningEndPointUri(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            return Configure(options => options.WarningEndPointUri = endpoint);
        }
        
    }
}