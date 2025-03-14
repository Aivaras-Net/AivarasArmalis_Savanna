using Savanna.Core;
using Savanna.Core.Interfaces;

namespace Savanna.Web.Services.Interfaces
{
    /// <summary>
    /// Service handling game logic and state management
    /// </summary>
    public interface IGameService
    {
        /// <summary>
        /// Gets whether the game is currently running
        /// </summary>
        bool IsGameRunning { get; }

        /// <summary>
        /// Gets whether the game is currently paused
        /// </summary>
        bool IsPaused { get; }

        /// <summary>
        /// Gets the game engine instance
        /// </summary>
        GameEngine GameEngine { get; }

        /// <summary>
        /// Gets the game logs
        /// </summary>
        IReadOnlyList<string> GameLogs { get; }

        /// <summary>
        /// Gets the field width
        /// </summary>
        int FieldWidth { get; }

        /// <summary>
        /// Gets the field height
        /// </summary>
        int FieldHeight { get; }

        /// <summary>
        /// Starts a new game
        /// </summary>
        /// <param name="renderer">The renderer to use</param>
        void StartNewGame(IConsoleRenderer renderer);

        /// <summary>
        /// Stops the current game
        /// </summary>
        void StopGame();

        /// <summary>
        /// Toggles between paused and running states
        /// </summary>
        void TogglePause();

        /// <summary>
        /// Spawns a new antelope at a random position
        /// </summary>
        void SpawnAntelope();

        /// <summary>
        /// Spawns a new lion at a random position
        /// </summary>
        void SpawnLion();

        /// <summary>
        /// Updates the game state
        /// </summary>
        void Update();

        /// <summary>
        /// Logs a message to the game log
        /// </summary>
        /// <param name="message">The message to log</param>
        void LogMessage(string message);

        /// <summary>
        /// Starts the game timer
        /// </summary>
        void StartTimer();

        /// <summary>
        /// Stops the game timer
        /// </summary>
        void StopTimer();

        /// <summary>
        /// Event raised when the game state changes
        /// </summary>
        event EventHandler GameStateChanged;
    }
}