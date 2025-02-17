using System.Text;
using Savanna.Core.Interfaces;

namespace Savanna.CLI
{
    /// <summary>
    /// Provides console-based rendering functionality for displaying fields and messages.
    /// </summary>
    public class ConsoleRenderer : IConsoleRenderer
    {
        public int HeaderOffset { get; set; }
        private string _message;
        private int _messageFrames;

        public ConsoleRenderer(int headerOffset = 0)
        {
            HeaderOffset = headerOffset;
            _message = string.Empty;
            _messageFrames = 0;
        }

        /// <summary>
        /// Displays a temporary message for a specified number of frames.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="frames">The duration (in frames) for which the message is visible.</param>
        public void ShowMessage(string message, int frames)
        {
            _message = message;
            _messageFrames = frames;
        }

        /// <summary>
        /// Renders the field with borders and an optional message.
        /// </summary>
        /// <param name="field">A two-dimensional character array representing the field to render.</param>
        public void RenderField(char[,] field)
        {
            int rows = field.GetLength(0);
            int cols = field.GetLength(1);

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

            int messageLine = HeaderOffset +2 + rows;
            Console.SetCursorPosition(0, messageLine);
            if (_messageFrames > 0)
            {
                Console.Write(_message.PadRight(cols + 2));
                _messageFrames--;
            }
            else
            {
                Console.Write(new string(ConsoleConstants.FieldFill, cols + 2));
            }
        }
    }
}
