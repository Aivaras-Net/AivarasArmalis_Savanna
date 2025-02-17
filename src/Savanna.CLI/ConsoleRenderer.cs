using System.Text;
using Savanna.Core.Interfaces;

namespace Savanna.CLI
{
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

        public void ShowMessage(string message, int frames)
        {
            _message = message;
            _messageFrames = frames;
        }

        public void RenderField(char[,] field)
        {
            int rows = field.GetLength(0);
            int cols = field.GetLength(1);

            string topBorder = "+" + new string('-', cols) + "+";
            Console.SetCursorPosition(0, HeaderOffset);
            Console.Write(topBorder);

            for (int i = 0; i < rows; i++)
            {
                var sb = new StringBuilder();
                sb.Append("|");
                for (int j = 0; j < cols; j++)
                {
                    sb.Append(field[i, j]);
                }
                sb.Append("|");
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
                Console.Write(new string(' ', cols + 2));
            }
        }
    }
}
