namespace Savanna.CLI.Interfaces
{
    /// <summary>
    /// Defines the logging service for the game
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// Adds a new log entry
        /// </summary>
        /// <param name="message">The log message</param>
        /// <param name="durationInFrames">The number of frames to display the message</param>
        void AddLogEntry(string message, int durationInFrames);

        /// <summary>
        /// Updates the log queue for the current frame
        /// </summary>
        void UpdateLogs();

        /// <summary>
        /// Gets all current log entries to display
        /// </summary>
        /// <param name="maxEntries">Maximum number of entries to return</param>
        /// <returns>An array of log entries to display</returns>
        (string Message, int RemainingFrames, int FrameCreated)[] GetCurrentLogs(int maxEntries);

        /// <summary>
        /// Gets the current frame counter value
        /// </summary>
        /// <returns>The current frame number</returns>
        int GetCurrentFrame();
    }
}