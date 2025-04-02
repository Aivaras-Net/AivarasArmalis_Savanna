using System.ComponentModel.DataAnnotations;

namespace Savanna.Web.Models
{
    /// <summary>
    /// View model for game save operations
    /// </summary>
    public class GameSaveViewModel
    {
        /// <summary>
        /// ID of the save (used for loading/deleting)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the save
        /// </summary>
        [Required(ErrorMessage = "Save name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Save name must be between 3 and 50 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// When the save was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Formatted date/time for display
        /// </summary>
        public string FormattedDate => CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
    }
}