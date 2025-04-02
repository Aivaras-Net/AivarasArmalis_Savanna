using Microsoft.EntityFrameworkCore;
using Savanna.Web.Data;
using Savanna.Web.Models;
using Savanna.Web.Services.Interfaces;

namespace Savanna.Web.Services
{
    /// <summary>
    /// Service for managing game saves in the database
    /// </summary>
    public class GameSaveService : IGameSaveService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<GameSaveService> _logger;

        public GameSaveService(
            ApplicationDbContext dbContext,
            ILogger<GameSaveService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<List<GameSave>> GetSavesForUserAsync(string userId)
        {
            return await _dbContext.GameSaves
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<GameSave?> GetSaveByIdAsync(int saveId)
        {
            return await _dbContext.GameSaves
                .FirstOrDefaultAsync(s => s.Id == saveId);
        }

        /// <inheritdoc />
        public async Task<GameSave> CreateSaveAsync(string saveName, string gameStateJson, string userId)
        {
            var gameSave = new GameSave
            {
                Name = saveName,
                GameStateJson = gameStateJson,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.GameSaves.AddAsync(gameSave);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Created game save {SaveId} for user {UserId}", gameSave.Id, userId);
            return gameSave;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteSaveAsync(int saveId, string userId)
        {
            var save = await _dbContext.GameSaves
                .FirstOrDefaultAsync(s => s.Id == saveId && s.UserId == userId);

            if (save == null)
            {
                _logger.LogWarning("Failed to delete save {SaveId}. Save not found or doesn't belong to user {UserId}", saveId, userId);
                return false;
            }

            _dbContext.GameSaves.Remove(save);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Deleted game save {SaveId} for user {UserId}", saveId, userId);
            return true;
        }
    }
}