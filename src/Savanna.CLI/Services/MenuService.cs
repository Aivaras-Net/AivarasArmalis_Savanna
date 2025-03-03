using Savanna.CLI.Interfaces;
using Savanna.Core.Constants;
using Savanna.Core.Interfaces;

namespace Savanna.CLI.Services
{
    /// <summary>
    /// Implementation of the menu service for console-based user interactions
    /// </summary>
    public class MenuService : IMenuService
    {
        private readonly IRendererService _renderer;
        private Dictionary<ConsoleKey, string> _animalKeyMappings;

        public MenuService(IRendererService renderer)
        {
            _renderer = renderer;
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
            Console.CursorVisible = false;
            int currentSelection = 0;

            ConsoleKey key;
            do
            {
                _renderer.RenderMenu(title, options, currentSelection);

                key = Console.ReadKey(true).Key;

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
            Console.CursorVisible = true;

            while (true)
            {
                Console.WriteLine($"{prompt} [{minValue}-{maxValue}, default: {defaultValue}]: ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    return defaultValue;
                }

                if (int.TryParse(input, out int result) && result >= minValue && result <= maxValue)
                {
                    return result;
                }

                Console.WriteLine($"Please enter a number between {minValue} and {maxValue}.");
            }
        }

        /// <summary>
        /// Displays the game command guide
        /// </summary>
        public void DisplayCommandGuide()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, ConsoleConstants.HeaderHeight);
            Console.WriteLine("Available animals:");

            int line = ConsoleConstants.HeaderHeight + 1;

            foreach (var mapping in _animalKeyMappings)
            {
                DisplayAnimalCommand(mapping.Key, mapping.Value, ref line);
            }

            Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
            Console.SetCursorPosition(0, line + 1);
            Console.WriteLine("Commands:");
            Console.SetCursorPosition(0, line + 2);
            Console.WriteLine("[S] - Save game");
            Console.SetCursorPosition(0, line + 3);
            Console.WriteLine("[Space] - Pause/Resume");
            Console.SetCursorPosition(0, line + 4);
            Console.WriteLine("[Esc] - Return to main menu");
        }

        /// <summary>
        /// Displays a single animal command with colored animal name
        /// </summary>
        private void DisplayAnimalCommand(ConsoleKey key, string animalName, ref int line)
        {
            Console.SetCursorPosition(0, line);
            Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
            Console.Write($"[{key}] - Add ");

            Console.ForegroundColor = _renderer.GetAnimalColor(animalName);
            Console.WriteLine(animalName);

            line++;
        }

        /// <summary>
        /// Clears the console screen
        /// </summary>
        public void ClearScreen()
        {
            Console.Clear();
        }

        /// <summary>
        /// Gets custom animal key mappings
        /// </summary>
        private Dictionary<ConsoleKey, string> GetCustomAnimalMappings()
        {
            return new Dictionary<ConsoleKey, string>();
        }
    }
}