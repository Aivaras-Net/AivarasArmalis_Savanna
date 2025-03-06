namespace Savanna.CLI.Interfaces
{
    /// <summary>
    /// Provides an abstraction over console operations for better testability and separation of concerns
    /// </summary>
    public interface IConsoleWrapper
    {
        /// <summary>
        /// Gets a value indicating whether a key press is available in the input stream
        /// </summary>
        bool KeyAvailable { get; }

        /// <summary>
        /// Gets or sets the cursor visibility
        /// </summary>
        bool CursorVisible { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the console
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Reads the next key pressed, optionally displaying the pressed key
        /// </summary>
        /// <param name="intercept">If true, the pressed key will not be displayed</param>
        /// <returns>The key that was pressed</returns>
        ConsoleKey ReadKey(bool intercept);

        /// <summary>
        /// Reads a line of text from the console
        /// </summary>
        /// <returns>The line that was read</returns>
        string ReadLine();

        /// <summary>
        /// Sets the position of the cursor
        /// </summary>
        /// <param name="left">The column position</param>
        /// <param name="top">The row position</param>
        void SetCursorPosition(int left, int top);

        /// <summary>
        /// Writes text to the console without a line terminator
        /// </summary>
        /// <param name="value">The text to write</param>
        void Write(string value);

        /// <summary>
        /// Writes text to the console with a line terminator
        /// </summary>
        /// <param name="value">The text to write</param>
        void WriteLine(string value);

        /// <summary>
        /// Clears the console screen
        /// </summary>
        void Clear();
    }
}