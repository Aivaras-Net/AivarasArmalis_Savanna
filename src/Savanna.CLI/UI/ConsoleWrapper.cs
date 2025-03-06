using Savanna.CLI.Interfaces;

namespace Savanna.CLI.UI
{
    /// <summary>
    /// Default implementation of IConsoleWrapper that wraps the System.Console functionality
    /// </summary>
    public class ConsoleWrapper : IConsoleWrapper
    {
        public bool KeyAvailable => Console.KeyAvailable;

        public bool CursorVisible
        {
            get => Console.CursorVisible;
            set => Console.CursorVisible = value;
        }

        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        public ConsoleKey ReadKey(bool intercept) => Console.ReadKey(intercept).Key;

        public string ReadLine() => Console.ReadLine() ?? string.Empty;

        public void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);

        public void Write(string value) => Console.Write(value);

        public void WriteLine(string value) => Console.WriteLine(value);

        public void Clear() => Console.Clear();
    }
}