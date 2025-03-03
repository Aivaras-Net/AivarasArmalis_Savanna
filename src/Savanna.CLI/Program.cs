using Savanna.Core;
using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;
using System.Reflection;

namespace Savanna.CLI
{
    internal class Program
    {
        private static Dictionary<ConsoleKey, string> _animalKeyMappings = new();
        private static readonly ConsoleKey[] _availableKeys = new[]
        {
            ConsoleKey.A, ConsoleKey.B, ConsoleKey.C, ConsoleKey.D, ConsoleKey.E, ConsoleKey.F, ConsoleKey.G,
            ConsoleKey.H, ConsoleKey.I, ConsoleKey.J, ConsoleKey.K, ConsoleKey.L, ConsoleKey.M, ConsoleKey.N,
            ConsoleKey.O, ConsoleKey.P, ConsoleKey.R, ConsoleKey.S, ConsoleKey.T, ConsoleKey.U, ConsoleKey.V,
            ConsoleKey.W, ConsoleKey.X, ConsoleKey.Y, ConsoleKey.Z
        };
        private static ConsoleRenderer _renderer;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            string currentDirectory = Directory.GetCurrentDirectory();
            string projectRoot = Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\..\..\.."));
            string importsFolder = Path.Combine(projectRoot, "Imports");

            if (!Directory.Exists(importsFolder))
            {
                Directory.CreateDirectory(importsFolder);
            }

            int fieldWidth = GetFieldDimension("width", GameConstants.DefaultFieldWidth);
            int fieldHeight = GetFieldDimension("height", GameConstants.DefaultFieldHeight);

            _renderer = new ConsoleRenderer(ConsoleConstants.TotalHeaderOffset);
            GameEngine engine = new GameEngine(_renderer, fieldWidth, fieldHeight);

            int requiredWidth = Math.Max(fieldWidth + 5, 60);
            int requiredHeight = Math.Max(_renderer.GetTotalDisplayHeight(fieldHeight), 20);

            try
            {
                Console.WindowWidth = Math.Max(Console.WindowWidth, requiredWidth);
                Console.WindowHeight = Math.Max(Console.WindowHeight, requiredHeight);
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"Warning: Console window might be too small for the selected field size.");
                Console.WriteLine($"Recommended minimum size: {requiredWidth}x{requiredHeight}");
                Console.WriteLine("Press any key to continue anyway...");
                Console.ReadKey(true);
                Console.Clear();
            }

            AssignKeyForAnimal(GameConstants.AntelopeName);
            AssignKeyForAnimal(GameConstants.LionName);
            _renderer.RegisterAnimalColor(GameConstants.AntelopeName);
            _renderer.RegisterAnimalColor(GameConstants.LionName);

            string[] dllFiles = Directory.GetFiles(importsFolder, "*.dll");

            foreach (string dllFile in dllFiles)
            {
                try
                {
                    Assembly customAssembly = Assembly.LoadFrom(dllFile);

                    foreach (Type type in customAssembly.GetTypes()
                        .Where(t => typeof(IAnimalBehavior).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract))
                    {
                        var behavior = (IAnimalBehavior)Activator.CreateInstance(type);
                        AnimalFactory.RegisterBehavior(behavior);
                        AssignKeyForAnimal(behavior.AnimalName);
                        _renderer.RegisterAnimalColor(behavior.AnimalName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading assembly {Path.GetFileName(dllFile)}: {ex.Message}");
                }
            }
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(ConsoleConstants.Header);
            DisplayCommandGuide();

            bool running = true;

            while (running)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Q)
                    {
                        running = false;
                        continue;
                    }

                    if (_animalKeyMappings.TryGetValue(key, out string animalName))
                    {
                        var animal = AnimalFactory.CreateAnimal(animalName);
                        engine.AddAnimal(animal);
                    }
                }

                engine.Update();
                DisplayCommandGuide();
                engine.DrawField();
                Thread.Sleep(ConsoleConstants.IterationDuration);
            }
        }

        private static void AssignKeyForAnimal(string animalName)
        {
            ConsoleKey preferredKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), animalName[0].ToString().ToUpper());

            if (!_animalKeyMappings.ContainsValue(animalName))
            {
                if (_availableKeys.Contains(preferredKey) && !_animalKeyMappings.ContainsKey(preferredKey))
                {
                    _animalKeyMappings[preferredKey] = animalName;
                    return;
                }

                foreach (var key in _availableKeys)
                {
                    if (!_animalKeyMappings.ContainsKey(key))
                    {
                        _animalKeyMappings[key] = animalName;
                        break;
                    }
                }
            }
        }

        private static void DisplayCommandGuide()
        {
            Console.SetCursorPosition(0, ConsoleConstants.HeaderHeight);
            Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
            Console.WriteLine("Available animals:");

            int line = ConsoleConstants.HeaderHeight + 1;
            foreach (var mapping in _animalKeyMappings)
            {
                Console.SetCursorPosition(0, line);
                Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
                Console.Write($"[{mapping.Key}] - Spawn ");

                string animalName = mapping.Value;
                Console.ForegroundColor = _renderer.GetAnimalColor(animalName);
                Console.WriteLine(animalName);
                line++;
            }

            Console.SetCursorPosition(0, line++);
            Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
            Console.WriteLine("[Q] - Quit the game");

            for (int i = line; i < ConsoleConstants.TotalHeaderOffset; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }

            Console.ForegroundColor = ConsoleConstants.DefaultFieldColor;
        }

        private static int GetFieldDimension(string dimensionName, int defaultValue)
        {
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine($"Enter field {dimensionName} (default: {defaultValue}, minimum: 5, press Enter to use default):");
            string input = Console.ReadLine();
            Console.CursorVisible = false;

            if (string.IsNullOrWhiteSpace(input))
            {
                return defaultValue;
            }

            if (int.TryParse(input, out int dimension) && dimension >= 5)
            {
                if (dimensionName == "width" && dimension > Console.LargestWindowWidth - 5)
                {
                    Console.WriteLine($"Requested width is too large for your console. Maximum allowed: {Console.LargestWindowWidth - 5}");
                    Console.WriteLine($"Using maximum allowed width: {Console.LargestWindowWidth - 5}");
                    Thread.Sleep(2000);
                    return Console.LargestWindowWidth - 5;
                }
                else if (dimensionName == "height" && dimension > Console.LargestWindowHeight - ConsoleConstants.TotalHeaderOffset - 7)
                {
                    int maxHeight = Console.LargestWindowHeight - ConsoleConstants.TotalHeaderOffset - 7;
                    Console.WriteLine($"Requested height is too large for your console. Maximum allowed: {maxHeight}");
                    Console.WriteLine($"Using maximum allowed height: {maxHeight}");
                    Thread.Sleep(2000);
                    return maxHeight;
                }
                return dimension;
            }
            else
            {
                Console.WriteLine($"Invalid input. Using default {dimensionName}: {defaultValue}");
                Thread.Sleep(1500);
                return defaultValue;
            }
        }
    }
}
