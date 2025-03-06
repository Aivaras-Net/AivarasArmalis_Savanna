namespace Savanna.CLI.Interfaces
{
    /// <summary>
    /// Defines the rendering operations for menu-related UI elements
    /// </summary>
    public interface IMenuRenderer
    {
        /// <summary>
        /// Renders a menu with the given options
        /// </summary>
        /// <param name="title">The menu title</param>
        /// <param name="options">The menu options</param>
        /// <param name="selectedIndex">The currently selected index</param>
        void RenderMenu(string title, string[] options, int selectedIndex);

        /// <summary>
        /// Renders a header with the specified text
        /// </summary>
        /// <param name="text">The header text</param>
        void RenderHeader(string text);

        /// <summary>
        /// Displays the game command guide
        /// </summary>
        /// <returns>The total height used by the command guide</returns>
        int DisplayCommandGuide();

        /// <summary>
        /// Gets the total height needed for the command guide
        /// </summary>
        /// <returns>The height needed for the command guide</returns>
        int GetCommandGuideHeight();

        /// <summary>
        /// Clears the console screen
        /// </summary>
        void ClearScreen();
    }
}