using Savanna.CLI.Interfaces;

namespace Savanna.CLI.Services
{
    /// <summary>
    /// Implements the logging service for the game
    /// </summary>
    public class LogService : ILogService
    {
        private readonly Queue<(string Message, int RemainingFrames, int FrameCreated)> _logQueue =
            new Queue<(string, int, int)>();

        private int _frameCounter = 0;
        private readonly int _maxLogCapacity;

        public LogService(int maxLogCapacity = 15)
        {
            _maxLogCapacity = maxLogCapacity;
        }

        /// <summary>
        /// Adds a new log entry to the queue
        /// </summary>
        /// <param name="message">The log message</param>
        /// <param name="durationInFrames">How long to display the message</param>
        public void AddLogEntry(string message, int durationInFrames)
        {
            _logQueue.Enqueue((message, durationInFrames, _frameCounter));

            while (_logQueue.Count > _maxLogCapacity)
            {
                _logQueue.Dequeue();
            }
        }

        /// <summary>
        /// Updates all logs in the queue, decrementing their duration frames
        /// </summary>
        public void UpdateLogs()
        {
            _frameCounter++;

            int count = _logQueue.Count;
            for (int i = 0; i < count; i++)
            {
                var (message, frames, frameCreated) = _logQueue.Dequeue();
                if (frames > 1)
                {
                    _logQueue.Enqueue((message, frames - 1, frameCreated));
                }
            }
        }

        /// <summary>
        /// Gets the most recent logs to display
        /// </summary>
        /// <param name="maxEntries">Maximum number of entries to return</param>
        /// <returns>Array of log entries</returns>
        public (string Message, int RemainingFrames, int FrameCreated)[] GetCurrentLogs(int maxEntries)
        {
            return _logQueue.TakeLast(Math.Min(_logQueue.Count, maxEntries)).ToArray();
        }

        /// <summary>
        /// Gets the current frame counter
        /// </summary>
        /// <returns>The current frame</returns>
        public int GetCurrentFrame()
        {
            return _frameCounter;
        }
    }
}