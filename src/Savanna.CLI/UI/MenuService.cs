using Savanna.CLI.Interfaces;
using Savanna.Core.Constants;
using Savanna.Core.Interfaces;

namespace Savanna.CLI.UI
{
    /// <summary>
    /// Implementation of menu-related services for console-based user interactions
    /// </summary>
    public class MenuService : IMenuRenderer, IMenuInteraction
    {
        private readonly IRendererService _renderer;
        private readonly IConsoleWrapper _console;
        private Dictionary<ConsoleKey, string> _animalKeyMappings;

        public MenuService(IRendererService renderer, IConsoleWrapper console)
        {
            _renderer = renderer;
            _console = console;
            _animalKeyMappings = new Dictionary<ConsoleKey, string>();
        }

        /// <summary>
        /// Updates the animal key mappings dictionary
        /// </summary>
        /// <param name="keyMappings">Current key mappings for animals</param>
        public void UpdateAnimalKeyMappings(Dictionary<ConsoleKey, string> keyMappings)
        {
            _animalKeyMappings = keyMappings;
        }

        /// <summary>
        /// Displays a menu and gets the user's selection
        /// </summary>
        /// <param name="title">The menu title</param>
        /// <param name="options">The menu options</param>
        /// <returns>The selected index</returns>
        public int GetSelectionFromOptions(string title, string[] options)
        {
            _console.CursorVisible = false;
            int currentSelection = 0;

            ConsoleKey key;
            do
            {
                _renderer.RenderMenu(title, options, currentSelection);

                key = _console.ReadKey(true);

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        currentSelection = (currentSelection - 1 + options.Length) % options.Length;
                        break;
                    case ConsoleKey.DownArrow:
                        currentSelection = (currentSelection + 1) % options.Length;
                        break;
                }
            } while (key != ConsoleKey.Enter);

            return currentSelection;
        }

        /// <summary>
        /// Gets a numeric input from the user
        /// </summary>
        /// <param name="prompt">The prompt to display</param>
        /// <param name="defaultValue">Default value if the user doesn't enter anything</param>
        /// <param name="minValue">Minimum allowed value</param>
        /// <param name="maxValue">Maximum allowed value</param>
        /// <returns>The user's input as an integer</returns>
        public int GetNumericInput(string prompt, int defaultValue, int minValue, int maxValue)
        {
            _console.CursorVisible = true;

            while (true)
            {
                _console.WriteLine($"{prompt} [{minValue}-{maxValue}, default: {defaultValue}]: ");
                string input = _console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    return defaultValue;
                }

                if (int.TryParse(input, out int result) && result >= minValue && result <= maxValue)
                {
                    return result;
                }

                _console.WriteLine(string.Format(ConsoleConstants.NumericInputErrorFormat, minValue, maxValue));
            }
        }

        /// <summary>
        /// Displays the game command guide
        /// </summary>
        /// <returns>The total height used by the command guide</returns>
        public int DisplayCommandGuide()
        {
            int currentLine = ConsoleConstants.HeaderHeight;

            _console.ForegroundColor = ConsoleColor.Yellow;
            _console.SetCursorPosition(0, currentLine);
            _console.WriteLine(ConsoleConstants.AvailableAnimals);
            currentLine++;

            foreach (var mapping in _animalKeyMappings)
            {
                DisplayAnimalCommand(mapping.Key, mapping.Value, ref currentLine);
            }

            _console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
            _console.SetCursorPosition(0, currentLine + 1);
            _console.WriteLine(ConsoleConstants.CommandsHeader);
            currentLine += 2;

            _console.SetCursorPosition(0, currentLine);
            _console.WriteLine(ConsoleConstants.SaveGameCommand);
            currentLine++;

            _console.SetCursorPosition(0, currentLine);
            _console.WriteLine(ConsoleConstants.PauseResumeCommand);
            currentLine++;

            _console.SetCursorPosition(0, currentLine);
            _console.WriteLine(ConsoleConstants.ExitCommand);
            currentLine++;

            return currentLine - ConsoleConstants.HeaderHeight;
        }

        /// <summary>
        /// Gets the total height needed for the command guide
        /// </summary>
        /// <returns>The height needed for the command guide</returns>
        public int GetCommandGuideHeight()
        {
            // Calculate height:
            // 1 line for "Available animals:" header
            // 1 line per animal mapping
            // 1 empty line
            // 1 line for "Commands:" header
            // 3 lines for standard commands (Save, Pause/Resume, Exit)
            return 1 + _animalKeyMappings.Count + 1 + 1 + 3;
        }

        /// <summary>
        /// Displays a single animal command with colored animal name
        /// </summary>
        private void DisplayAnimalCommand(ConsoleKey key, string animalName, ref int line)
        {
            _console.SetCursorPosition(0, line);
            _console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
            _console.Write(string.Format(ConsoleConstants.AddAnimalCommandFormat, key, animalName));

            _console.ForegroundColor = _renderer.GetAnimalColor(animalName);
            _console.WriteLine(animalName);

            line++;
        }

        /// <summary>
        /// Clears the console screen
        /// </summary>
        public void ClearScreen()
        {
            _console.Clear();
        }

        public void RenderMenu(string title, string[] options, int selectedIndex)
        {
            _renderer.RenderMenu(title, options, selectedIndex);
        }

        public void RenderHeader(string text)
        {
            _renderer.RenderHeader(text);
        }
    }
}