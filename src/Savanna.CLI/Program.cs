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
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading assembly {Path.GetFileName(dllFile)}: {ex.Message}");
                }
            }

            AssignKeyForAnimal(GameConstants.AntelopeName);
            AssignKeyForAnimal(GameConstants.LionName);

            ConsoleRenderer renderer = new ConsoleRenderer(ConsoleConstants.TotalHeaderOffset);
            GameEngine engine = new GameEngine(renderer);

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
            Console.WriteLine("Available animals:");

            int line = ConsoleConstants.HeaderHeight + 1;
            foreach (var mapping in _animalKeyMappings)
            {
                Console.SetCursorPosition(0, line++);
                Console.WriteLine($"[{mapping.Key}] - Spawn {mapping.Value}");
            }

            Console.SetCursorPosition(0, line++);
            Console.WriteLine("[Q] - Quit the game");

            for (int i = line; i < ConsoleConstants.TotalHeaderOffset; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }
        }
    }
}
