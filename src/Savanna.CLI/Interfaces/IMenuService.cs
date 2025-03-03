namespace Savanna.CLI.Interfaces
{
    /// <summary>
    /// Defines the menu service for handling user interactions
    /// </summary>
    public interface IMenuService
    {
        /// <summary>
        /// Displays a menu and gets the user's selection
        /// </summary>
        /// <param name="title">The menu title</param>
        /// <param name="options">The menu options</param>
        /// <returns>The index of the selected option</returns>
        int GetSelectionFromOptions(string title, string[] options);

        /// <summary>
        /// Gets a numeric input from the user
        /// </summary>
        /// <param name="prompt">The prompt to display</param>
        /// <param name="defaultValue">The default value</param>
        /// <param name="minValue">The minimum allowed value</param>
        /// <param name="maxValue">The maximum allowed value</param>
        /// <returns>The user's input as an integer</returns>
        int GetNumericInput(string prompt, int defaultValue, int minValue, int maxValue);

        /// <summary>
        /// Displays the game command guide
        /// </summary>
        void DisplayCommandGuide();

        /// <summary>
        /// Clears the console screen
        /// </summary>
        void ClearScreen();
    }
}