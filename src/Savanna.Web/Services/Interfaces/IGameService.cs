using Savanna.Core;
using Savanna.Core.Interfaces;
using Savanna.Domain.Interfaces;
using Savanna.Web.Models;

namespace Savanna.Web.Services.Interfaces
{
    /// <summary>
    /// Service for managing game state and logic
    /// </summary>
    public interface IGameService
    {
        /// <summary>
        /// Current game engine
        /// </summary>
        GameEngine GameEngine { get; }

        /// <summary>
        /// Whether the game is currently running
        /// </summary>
        bool IsGameRunning { get; }

        /// <summary>
        /// Whether the game is paused
        /// </summary>
        bool IsPaused { get; }

        /// <summary>
        /// Whether to use letter display or icons
        /// </summary>
        bool UseLetterDisplay { get; set; }

        /// <summary>
        /// Game logs
        /// </summary>
        IReadOnlyList<string> GameLogs { get; }

        /// <summary>
        /// Width of the field
        /// </summary>
        int FieldWidth { get; }

        /// <summary>
        /// Height of the field
        /// </summary>
        int FieldHeight { get; }

        /// <summary>
        /// Selected animal details
        /// </summary>
        AnimalDetailViewModel? SelectedAnimalDetails { get; }

        /// <summary>
        /// Starts a new game
        /// </summary>
        /// <param name="renderer">Console renderer to use</param>
        void StartNewGame(IConsoleRenderer renderer);

        /// <summary>
        /// Stops the current game
        /// </summary>
        void StopGame();

        /// <summary>
        /// Toggles pause state
        /// </summary>
        void TogglePause();

        /// <summary>
        /// Spawns an antelope at a random position
        /// </summary>
        void SpawnAntelope();

        /// <summary>
        /// Spawns a lion at a random position
        /// </summary>
        void SpawnLion();

        /// <summary>
        /// Toggles display mode between letters and icons
        /// </summary>
        void ToggleDisplayMode();

        /// <summary>
        /// Updates the game state
        /// </summary>
        void Update();

        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="message">Message to log</param>
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
        /// Serializes the current game state to JSON
        /// </summary>
        /// <returns>JSON string representing the game state</returns>
        string SerializeGameState();

        /// <summary>
        /// Loads a game state from JSON
        /// </summary>
        /// <param name="json">JSON string representing the game state</param>
        /// <param name="renderer">Console renderer to use</param>
        void LoadGameState(string json, IConsoleRenderer renderer);

        /// <summary>
        /// Selects an animal by its position in the field
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>True if an animal was selected, false otherwise</returns>
        bool SelectAnimalAt(int x, int y);

        /// <summary>
        /// Deselects the currently selected animal (if any)
        /// </summary>
        void DeselectAnimal();

        /// <summary>
        /// Event fired when the game state changes
        /// </summary>
        event EventHandler GameStateChanged;

        /// <summary>
        /// Event fired when an animal is selected or deselected
        /// </summary>
        event EventHandler<AnimalDetailViewModel?> AnimalSelectionChanged;
    }
}