using Savanna.Web.Constants;
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
        public string FormattedDate => CreatedAt.ToString(WebConstants.SaveDateFormat);

        /// <summary>
        /// Iteration or age of the oldest animal in the game (approximation of game iterations)
        /// </summary>
        public int IterationCount { get; set; }

        /// <summary>
        /// Number of lions in the game
        /// </summary>
        public int LionCount { get; set; }

        /// <summary>
        /// Number of antelopes in the game
        /// </summary>
        public int AntelopeCount { get; set; }

        /// <summary>
        /// Total number of animals in the game
        /// </summary>
        public int TotalAnimals { get; set; }

        /// <summary>
        /// Initializes a new instance of GameSaveViewModel with default values
        /// </summary>
        public GameSaveViewModel()
        {
            // Set default TotalAnimals to the sum of specific animal types for backward compatibility
            TotalAnimals = LionCount + AntelopeCount;
        }
    }
}