namespace Savanna.CLI
{
    internal static class ConsoleSelectionUtility
    {
        /// <summary>
        /// Handles arrow key selection from a list of options.
        /// </summary>
        /// <param name="prompt">The prompt message to display.</param>
        /// <param name="options">Array of option strings to display.</param>
        /// <returns>Index of the selected option.</returns>
        public static int GetSelectionFromOptions(string prompt, string[] options)
        {
            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine(prompt);

                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine((i == selectedIndex ? ConsoleConstants.ArrowPointer : ConsoleConstants.NoArrowPrefix) + options[i]);
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex + 1) % options.Length;
                        break;
                    case ConsoleKey.Enter:
                        return selectedIndex;
                }
            }
        }
    }
}