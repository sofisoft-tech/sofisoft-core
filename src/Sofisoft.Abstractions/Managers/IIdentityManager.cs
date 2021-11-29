namespace Sofisoft.Abstractions.Managers
{
    /// <summary>
    /// Interface for manage identity.
    /// </summary>
    public interface IIdentityManager
    {
        /// <summary>
        /// Get the id of the client application.
        /// </summary>
        string? ClientAppId { get; }

        /// <summary>
        /// Get the company id of the authenticated user.
        /// </summary>
        string? CompanyId { get; }

        /// <summary>
        /// Get the access Token.
        /// </summary>
        string Token { get; }

        /// <summary>
        /// Get the Token type.
        /// </summary>
        string TokenType { get; }

        /// <summary>
        /// Get the id of the authenticated user.
        /// </summary>
        string? UserId { get; }

        /// <summary>
        /// Get the authenticated username.
        /// </summary>
        string? Username { get; }
    }
}