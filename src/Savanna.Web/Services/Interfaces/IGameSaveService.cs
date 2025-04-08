using Savanna.Web.Models;

namespace Savanna.Web.Services.Interfaces
{
    /// <summary>
    /// Service for managing game saves
    /// </summary>
    public interface IGameSaveService
    {
        /// <summary>
        /// Gets all saves for a user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>List of game saves</returns>
        Task<List<GameSave>> GetSavesForUserAsync(string userId);

        /// <summary>
        /// Gets a specific save by ID
        /// </summary>
        /// <param name="saveId">The save ID</param>
        /// <returns>The game save or null if not found</returns>
        Task<GameSave?> GetSaveByIdAsync(int saveId);

        /// <summary>
        /// Creates a new save
        /// </summary>
        /// <param name="saveName">Name of the save</param>
        /// <param name="gameStateJson">JSON serialized game state</param>
        /// <param name="userId">User ID who is creating the save</param>
        /// <returns>The created game save</returns>
        Task<GameSave> CreateSaveAsync(string saveName, string gameStateJson, string userId);

        /// <summary>
        /// Deletes a save
        /// </summary>
        /// <param name="saveId">The save ID to delete</param>
        /// <param name="userId">The user ID who is deleting the save (for validation)</param>
        /// <returns>True if deleted successfully, false otherwise</returns>
        Task<bool> DeleteSaveAsync(int saveId, string userId);
    }
}