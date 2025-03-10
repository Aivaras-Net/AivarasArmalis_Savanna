using Savanna.Core.Interfaces;

namespace Savanna.CLI.Interfaces
{
    /// <summary>
    /// Defines the renderer service responsible for all console output
    /// </summary>
    public interface IRendererService
    {
        /// <summary>
        /// Gets or sets the vertical offset for rendering the field
        /// </summary>
        int HeaderOffset { get; set; }

        /// <summary>
        /// Renders the game field
        /// </summary>
        /// <param name="field">The field representation to render</param>
        void RenderField(char[,] field);

        /// <summary>
        /// Renders a header with the specified text
        /// </summary>
        /// <param name="text">The header text</param>
        void RenderHeader(string text);

        /// <summary>
        /// Renders a menu with the given options
        /// </summary>
        /// <param name="title">The menu title</param>
        /// <param name="options">The menu options</param>
        /// <param name="selectedIndex">The currently selected index</param>
        void RenderMenu(string title, string[] options, int selectedIndex);

        /// <summary>
        /// Registers a color for a specific animal
        /// </summary>
        /// <param name="animalName">The name of the animal</param>
        void RegisterAnimalColor(string animalName);

        /// <summary>
        /// Gets the color assigned to an animal
        /// </summary>
        /// <param name="animalName">Name of the animal</param>
        /// <returns>The console color for the animal</returns>
        ConsoleColor GetAnimalColor(string animalName);

        /// <summary>
        /// Gets the total display height needed
        /// </summary>
        /// <param name="fieldHeight">The height of the field</param>
        /// <returns>The total height needed for display</returns>
        int GetTotalDisplayHeight(int fieldHeight);

        /// <summary>
        /// Shows a log message for a specified number of frames
        /// </summary>
        /// <param name="message">The message to show</param>
        /// <param name="frames">The number of frames to show it</param>
        void ShowLog(string message, int frames);
    }
}