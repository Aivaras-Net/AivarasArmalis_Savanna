using System.Text;
using Savanna.Core.Constants;
using Savanna.Core.Interfaces;

namespace Savanna.CLI
{
    /// <summary>
    /// Provides console-based rendering functionality for displaying fields and messages.
    /// </summary>
    public class ConsoleRenderer : IConsoleRenderer
    {
        public int HeaderOffset { get; set; }
        private readonly Queue<(string Message, int RemainingFrames)> _messageQueue = new Queue<(string, int)>();
        private readonly int _maxMessages = 5;
        private readonly int _messageAreaHeight;

        public ConsoleRenderer(int headerOffset = 0)
        {
            HeaderOffset = headerOffset;
            _messageAreaHeight = _maxMessages + 1;
        }

        /// <summary>
        /// Displays a temporary message for a specified number of frames.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="frames">The duration (in frames) for which the message is visible.</param>
        public void ShowMessage(string message, int frames)
        {
            _messageQueue.Enqueue((message, frames));

            while (_messageQueue.Count > _maxMessages * 2)
            {
                _messageQueue.Dequeue();
            }
        }

        /// <summary>
        /// Renders the field with borders and an optional message.
        /// </summary>
        /// <param name="field">A two-dimensional character array representing the field to render.</param>
        public void RenderField(char[,] field)
        {
            int rows = field.GetLength(0);
            int cols = field.GetLength(1);

            for (int i = HeaderOffset; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            string topBorder = ConsoleConstants.BorderCorner + new string(ConsoleConstants.HorizontalBorder, cols) + ConsoleConstants.BorderCorner;
            Console.SetCursorPosition(0, HeaderOffset);
            Console.Write(topBorder);

            for (int i = 0; i < rows; i++)
            {
                var sb = new StringBuilder();
                sb.Append(ConsoleConstants.VerticalBorder);
                for (int j = 0; j < cols; j++)
                {
                    sb.Append(field[i, j]);
                }
                sb.Append(ConsoleConstants.VerticalBorder);
                Console.SetCursorPosition(0, HeaderOffset + 1 + i);
                Console.Write(sb.ToString());
            }

            Console.SetCursorPosition(0, HeaderOffset + 1 + rows);
            Console.WriteLine(topBorder);

            UpdateMessageQueue();

            DrawMessageArea(cols, HeaderOffset + 2 + rows);
        }

        /// <summary>
        /// Updates the message queue by decrementing frame counters and removing expired messages.
        /// </summary>
        private void UpdateMessageQueue()
        {
            int count = _messageQueue.Count;
            for (int i = 0; i < count; i++)
            {
                var (message, frames) = _messageQueue.Dequeue();
                if (frames > 1)
                {
                    _messageQueue.Enqueue((message, frames - 1));
                }
            }
        }

        /// <summary>
        /// Draws the message area with all active messages.
        /// </summary>
        /// <param name="fieldWidth">Width of the game field for formatting.</param>
        /// <param name="startLine">Starting line for the message area.</param>
        private void DrawMessageArea(int fieldWidth, int startLine)
        {
            Console.SetCursorPosition(0, startLine);
            Console.WriteLine("Messages:");

            int line = startLine + 1;
            int displayCount = Math.Min(_messageQueue.Count, _maxMessages);
            var messagesToDisplay = _messageQueue.TakeLast(displayCount).ToArray();

            for (int i = 0; i < displayCount; i++)
            {
                Console.SetCursorPosition(0, line + i);
                Console.Write(messagesToDisplay[i].Message.PadRight(fieldWidth + 2));
            }

            for (int i = displayCount; i < _maxMessages; i++)
            {
                Console.SetCursorPosition(0, line + i);
                Console.Write(new string(' ', fieldWidth + 2));
            }
        }

        /// <summary>
        /// Gets the total height required for the console display.
        /// </summary>
        /// <param name="fieldHeight">Height of the game field.</param>
        /// <returns>Total height needed for the display.</returns>
        public int GetTotalDisplayHeight(int fieldHeight)
        {
            return HeaderOffset + fieldHeight + 2 + _messageAreaHeight; // Header + field + borders + message area
        }
    }
}
