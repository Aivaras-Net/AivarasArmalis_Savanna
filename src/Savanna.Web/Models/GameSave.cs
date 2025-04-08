using System;

namespace Savanna.Web.Models
{
    /// <summary>
    /// Represents a saved game state in the database
    /// </summary>
    public class GameSave
    {
        /// <summary>
        /// Unique identifier for the save
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the save
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// JSON serialized game state
        /// </summary>
        public string GameStateJson { get; set; } = string.Empty;

        /// <summary>
        /// When the game was saved
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// User who created the save
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the user
        /// </summary>
        public ApplicationUser User { get; set; } = null!;
    }
}