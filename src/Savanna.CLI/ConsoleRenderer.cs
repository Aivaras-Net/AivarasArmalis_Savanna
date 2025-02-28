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
        private readonly Dictionary<string, ConsoleColor> _animalColors = new Dictionary<string, ConsoleColor>();
        private readonly Random _random = new Random();

        public ConsoleRenderer(int headerOffset = 0)
        {
            HeaderOffset = headerOffset;
            _messageAreaHeight = _maxMessages + 1;

            _animalColors[GameConstants.AntelopeName] = ConsoleConstants.AntelopeColor;
            _animalColors[GameConstants.LionName] = ConsoleConstants.LionColor;
        }

        /// <summary>
        /// Registers a color for an animal type. If the animal is not one of the built-in types,
        /// it will be assigned a random color.
        /// </summary>
        /// <param name="animalName">The name of the animal</param>
        public void RegisterAnimalColor(string animalName)
        {
            if (!_animalColors.ContainsKey(animalName))
            {
                if (animalName == GameConstants.AntelopeName)
                {
                    _animalColors[animalName] = ConsoleConstants.AntelopeColor;
                }
                else if (animalName == GameConstants.LionName)
                {
                    _animalColors[animalName] = ConsoleConstants.LionColor;
                }
                else
                {
                    // Assign a random color for imported animals, excluding:
                    // - Black (0) and White (15) for readability
                    // - Green (ConsoleConstants.AntelopeColor) to avoid confusion with antelopes
                    // - Red (ConsoleConstants.LionColor) to avoid confusion with lions
                    ConsoleColor randomColor;
                    do
                    {
                        randomColor = (ConsoleColor)_random.Next(1, 15);
                    } while (randomColor == ConsoleColor.Black ||
                             randomColor == ConsoleColor.White ||
                             randomColor == ConsoleConstants.AntelopeColor ||
                             randomColor == ConsoleConstants.LionColor);

                    _animalColors[animalName] = randomColor;
                }
            }
        }

        /// <summary>
        /// Gets the color assigned to an animal.
        /// </summary>
        /// <param name="animalName">The name of the animal</param>
        /// <returns>The console color assigned to the animal, or the default field color if not found</returns>
        public ConsoleColor GetAnimalColor(string animalName)
        {
            if (_animalColors.TryGetValue(animalName, out ConsoleColor color))
            {
                return color;
            }
            return ConsoleConstants.DefaultFieldColor;
        }

        /// <summary>
        /// Displays a temporary message for a specified number of frames.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="frames">The duration (in frames) for which the message is visible.</param>
        public void ShowMessage(string message, int frames)
        {
            Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
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

            Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
            string topBorder = ConsoleConstants.BorderCorner + new string(ConsoleConstants.HorizontalBorder, cols) + ConsoleConstants.BorderCorner;
            Console.SetCursorPosition(0, HeaderOffset);
            Console.Write(topBorder);

            for (int i = 0; i < rows; i++)
            {
                Console.SetCursorPosition(0, HeaderOffset + 1 + i);
                Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
                Console.Write(ConsoleConstants.VerticalBorder);

                for (int j = 0; j < cols; j++)
                {
                    char cellChar = field[i, j];

                    if (cellChar != GameConstants.FieldFill)
                    {
                        string animalName = GetAnimalNameFromChar(cellChar);
                        if (_animalColors.TryGetValue(animalName, out ConsoleColor color))
                        {
                            Console.ForegroundColor = color;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
                    }

                    Console.Write(cellChar);
                }

                Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
                Console.Write(ConsoleConstants.VerticalBorder);
            }

            Console.SetCursorPosition(0, HeaderOffset + 1 + rows);
            Console.WriteLine(topBorder);

            UpdateMessageQueue();

            DrawMessageArea(cols, HeaderOffset + 2 + rows);

            Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
        }

        /// <summary>
        /// Gets the animal name based on the character representation.
        /// </summary>
        /// <param name="c">The character representing an animal</param>
        /// <returns>The name of the animal</returns>
        private string GetAnimalNameFromChar(char c)
        {
            foreach (var animalName in _animalColors.Keys)
            {
                if (animalName.Length > 0 && animalName[0] == c)
                {
                    return animalName;
                }
            }

            return c.ToString();
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
            Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
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
