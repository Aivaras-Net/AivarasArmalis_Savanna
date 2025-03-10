namespace Savanna.CLI.Interfaces
{
    /// <summary>
    /// Defines the user interaction operations for menu-related functionality
    /// </summary>
    public interface IMenuInteraction
    {
        /// <summary>
        /// Gets a selection from a list of options
        /// </summary>
        /// <param name="title">The menu title</param>
        /// <param name="options">The menu options</param>
        /// <returns>The selected index</returns>
        int GetSelectionFromOptions(string title, string[] options);

        /// <summary>
        /// Gets a numeric input from the user
        /// </summary>
        /// <param name="prompt">The prompt to display</param>
        /// <param name="defaultValue">Default value if the user doesn't enter anything</param>
        /// <param name="minValue">Minimum allowed value</param>
        /// <param name="maxValue">Maximum allowed value</param>
        /// <returns>The user's input as an integer</returns>
        int GetNumericInput(string prompt, int defaultValue, int minValue, int maxValue);

        /// <summary>
        /// Updates the animal key mappings dictionary
        /// </summary>
        /// <param name="keyMappings">Current key mappings for animals</param>
        void UpdateAnimalKeyMappings(Dictionary<ConsoleKey, string> keyMappings);
    }
}