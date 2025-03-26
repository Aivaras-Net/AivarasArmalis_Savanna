using Savanna.Core;
using Savanna.Core.Interfaces;
using Savanna.Domain.Interfaces;
using Savanna.Web.Models;

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
        /// Gets or sets whether to use letters instead of icons to display animals
        /// </summary>
        bool UseLetterDisplay { get; set; }

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
        /// Gets the currently selected animal details or null if none selected
        /// </summary>
        AnimalDetailViewModel? SelectedAnimalDetails { get; }

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
        /// Toggles between letter and icon display modes
        /// </summary>
        void ToggleDisplayMode();

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
        /// Gets details for the animal at the specified position
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>AnimalDetailViewModel or null if no animal is at that position</returns>
        AnimalDetailViewModel? GetAnimalDetailsAt(int x, int y);

        /// <summary>
        /// Event raised when the game state changes
        /// </summary>
        event EventHandler GameStateChanged;

        /// <summary>
        /// Event raised when an animal is selected or deselected
        /// </summary>
        event EventHandler<AnimalDetailViewModel?> AnimalSelectionChanged;
    }
}