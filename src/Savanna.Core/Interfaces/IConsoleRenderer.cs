namespace Savanna.Core.Interfaces
{
    /// <summary>
    /// Defines a renderer for the console.
    /// </summary>
    public interface IConsoleRenderer
    {
        /// <summary>
        /// Renders the specified field.
        /// </summary>
        /// <param name="field">A two-dimensional character array representing the field layout.</param>
        public void RenderField(char[,] field);

        /// <summary>
        /// Registers a color for an animal type.
        /// </summary>
        /// <param name="animalName">The name of the animal</param>
        public void RegisterAnimalColor(string animalName);

        /// <summary>
        /// Gets the color assigned to an animal.
        /// </summary>
        /// <param name="animalName">The name of the animal</param>
        /// <returns>The console color assigned to the animal, or the default field color if not found</returns>
        public ConsoleColor GetAnimalColor(string animalName);

        /// <summary>
        /// Displays a log entry for a specified number of frames.
        /// </summary>
        /// <param name="message">The log message to be displayed.</param>
        /// <param name="frames">The duration (in frames) for which the log is visible.</param>
        public void ShowLog(string message, int frames);
    }
}
