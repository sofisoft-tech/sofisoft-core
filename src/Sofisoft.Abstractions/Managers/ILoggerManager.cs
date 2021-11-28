namespace Sofisoft.Abstractions.Managers
{
    /// <summary>
    /// Interface for event registration.
    /// </summary>
    public interface ILoggerManager
    {
        HttpClient? HttpClient { get; set; }

        /// <summary>
        /// Registers an event of type error.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="trace">Traceability of the error.</param>
        /// <param name="username">User name.</param>
        /// <returns>Returns the Id of the registered event.</returns>
        Task<string?> ErrorAsync(string message, string trace, string username);

        /// <summary>
        /// Registers an event of type error.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="trace">Traceability of the error.</param>
        /// <param name="username">User name.</param>
        /// <param name="userAgent">Information of user agent.</param>
        /// <returns>Returns the Id of the registered event.</returns>
        Task<string?> ErrorAsync(string message, string trace, string username, string userAgent);

        /// <summary>
        /// Registers an event of type information.
        /// </summary>
        /// <param name="message">Information.</param>
        /// <param name="username">User name.</param>
        /// <returns>Returns the Id of the registered event.</returns>
        Task<string?> InformationAsync(string message, string username);

        /// <summary>
        /// Registers an event of type information.
        /// </summary>
        /// <param name="message">Information.</param>
        /// <param name="username">User name.</param>
        /// <param name="userAgent">Information of user agent.</param>
        /// <returns>Returns the Id of the registered event.</returns>
        Task<string?> InformationAsync(string message, string username, string userAgent);

        /// <summary>
        /// Registers an event of type warning.
        /// </summary>
        /// <param name="message">Warning message.</param>
        /// <param name="username">User name.</param>
        /// <returns>Returns the Id of the registered event.</returns>
        Task<string?> WarningAsync(string message, string username);

        /// <summary>
        /// Registers an event of type warning.
        /// </summary>
        /// <param name="message">Warning message.</param>
        /// <param name="username">User name.</param>
        /// <param name="userAgent">Information of user agent.</param>
        /// <returns>Returns the Id of the registered event.</returns>
        Task<string?> WarningAsync(string message, string username, string userAgent);
    }
}