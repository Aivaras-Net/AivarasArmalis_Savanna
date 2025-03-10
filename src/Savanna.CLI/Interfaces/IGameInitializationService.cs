using Savanna.Core;
using Savanna.Core.Infrastructure;

namespace Savanna.CLI.Interfaces
{
    /// <summary>
    /// Interface for service that handles game initialization operations
    /// </summary>
    public interface IGameInitializationService
    {
        /// <summary>
        /// Gets the dictionary of animal key mappings
        /// </summary>
        Dictionary<ConsoleKey, string> AnimalKeyMappings { get; }

        /// <summary>
        /// Starts a new game with user-defined dimensions
        /// </summary>
        /// <returns>A configured GameEngine instance</returns>
        GameEngine StartNewGame();

        /// <summary>
        /// Loads a saved game from available save files
        /// </summary>
        /// <returns>A loaded GameEngine instance or null if loading failed</returns>
        GameEngine LoadSavedGame();

        /// <summary>
        /// Loads plugins from the specified folder
        /// </summary>
        /// <param name="importsFolder">The folder containing plugins</param>
        void LoadPlugins(string importsFolder);

        /// <summary>
        /// Shows a message when no save files are found
        /// </summary>
        /// <param name="message">The message to display</param>
        void ShowNoSaveFilesMessage(string message);

        /// <summary>
        /// Assigns a keyboard key for an animal
        /// </summary>
        /// <param name="animalName">The name of the animal to assign a key to</param>
        void AssignKeyForAnimal(string animalName);

        /// <summary>
        /// Gets the AnimalFactory instance used by this service
        /// </summary>
        /// <returns>The AnimalFactory instance</returns>
        AnimalFactory GetAnimalFactory();
    }
}