using Savanna.CLI.Interfaces;
using Savanna.Core.Constants;
using Savanna.Core.Interfaces;

namespace Savanna.CLI.Services
{
    /// <summary>
    /// Implementation of the renderer service for console output
    /// </summary>
    public class RendererService : IRendererService, IConsoleRenderer
    {
        public int HeaderOffset { get; set; }
        private readonly Dictionary<string, ConsoleColor> _animalColors = new Dictionary<string, ConsoleColor>();
        private readonly Random _random = new Random();
        private readonly ILogService _logService;

        public RendererService(ILogService logService, int headerOffset = 0)
        {
            _logService = logService;
            HeaderOffset = headerOffset;

            // Initialize default colors
            _animalColors[GameConstants.AntelopeName] = ConsoleConstants.AntelopeColor;
            _animalColors[GameConstants.LionName] = ConsoleConstants.LionColor;
        }

        /// <summary>
        /// Registers a color for an animal type
        /// </summary>
        /// <param name="animalName">Name of the animal</param>
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
        /// Gets the color assigned to an animal
        /// </summary>
        /// <param name="animalName">Name of the animal</param>
        /// <returns>The console color for the animal</returns>
        public ConsoleColor GetAnimalColor(string animalName)
        {
            if (_animalColors.TryGetValue(animalName, out ConsoleColor color))
            {
                return color;
            }
            return ConsoleConstants.DefaultFieldColor;
        }

        /// <summary>
        /// Shows a log message
        /// </summary>
        /// <param name="message">The message to show</param>
        /// <param name="frames">Number of frames to display it</param>
        public void ShowLog(string message, int frames)
        {
            _logService.AddLogEntry(message, frames);
        }

        /// <summary>
        /// Renders the field with borders and logs
        /// </summary>
        /// <param name="field">The field to render</param>
        public void RenderField(char[,] field)
        {
            _logService.UpdateLogs();
            int rows = field.GetLength(0);
            int cols = field.GetLength(1);

            ClearDisplay();

            RenderBorderedField(field, rows, cols);

            DrawLogArea(cols, HeaderOffset + 2 + rows);

            Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
        }

        /// <summary>
        /// Renders a header with the specified text
        /// </summary>
        /// <param name="text">The header text</param>
        public void RenderHeader(string text)
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleConstants.LogHeaderColor;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
        }

        /// <summary>
        /// Renders a menu with the given options
        /// </summary>
        /// <param name="title">The menu title</param>
        /// <param name="options">The menu options</param>
        /// <param name="selectedIndex">The currently selected index</param>
        public void RenderMenu(string title, string[] options, int selectedIndex)
        {
            Console.CursorVisible = false;
            Console.Clear();

            Console.ForegroundColor = ConsoleConstants.LogHeaderColor;
            Console.WriteLine(title);
            Console.WriteLine();

            for (int i = 0; i < options.Length; i++)
            {
                string prefix = (i == selectedIndex) ? ConsoleConstants.ArrowPointer : ConsoleConstants.NoArrowPrefix;
                Console.ForegroundColor = (i == selectedIndex) ? ConsoleColor.Cyan : ConsoleConstants.DefaultFieldColor;
                Console.WriteLine($"{prefix}{options[i]}");
            }

            Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
        }

        /// <summary>
        /// Gets the total display height needed
        /// </summary>
        /// <param name="fieldHeight">Height of the field</param>
        /// <returns>Total height needed for display</returns>
        public int GetTotalDisplayHeight(int fieldHeight)
        {
            const int logAreaHeight = 6; // Header + 5 log lines
            return HeaderOffset + fieldHeight + 2 + logAreaHeight; // +2 for borders
        }

        #region Private Helper Methods

        private void ClearDisplay()
        {
            for (int i = HeaderOffset; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }

        private void RenderBorderedField(char[,] field, int rows, int cols)
        {
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
        }

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

        private void DrawLogArea(int fieldWidth, int startLine)
        {
            int maxLogs = 5;
            int frameCounter = _logService.GetCurrentFrame();

            Console.ForegroundColor = ConsoleConstants.LogHeaderColor;
            Console.SetCursorPosition(0, startLine);
            Console.WriteLine($"Game Log (Current Frame: {frameCounter}):");

            int line = startLine + 1;
            var logsToDisplay = _logService.GetCurrentLogs(maxLogs);
            int displayCount = logsToDisplay.Length;

            Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;

            for (int i = 0; i < displayCount; i++)
            {
                var (message, _, frameCreated) = logsToDisplay[i];

                Console.SetCursorPosition(0, line + i);

                string formattedLog = $"[Frame: {frameCreated}] {message}";
                Console.Write(formattedLog.PadRight(fieldWidth + 2));
            }

            for (int i = displayCount; i < maxLogs; i++)
            {
                Console.SetCursorPosition(0, line + i);
                Console.Write(new string(' ', fieldWidth + 2));
            }
        }

        #endregion
    }
}