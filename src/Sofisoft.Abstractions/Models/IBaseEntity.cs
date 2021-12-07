namespace Sofisoft.Abstractions.Models
{
    public interface IBaseEntity
    {
        /// <summary>
        /// Get the Id of entity.
        /// </summary>
        string? Id { get; }

        /// <summary>
        /// Get the creation date.
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// Get or set the user registration.
        /// </summary>
        string? CreatedBy { get; set; }

        /// <summary>
        /// Get the modification date.
        /// </summary>
        DateTime? ModifiedAt { get; }

        /// <summary>
        /// Get or set the user modification.
        /// </summary>
        string? ModifiedBy { get; set; }
    }
}