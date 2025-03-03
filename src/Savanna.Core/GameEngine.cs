using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;
using Savanna.Core.Interfaces;
using System.Text.Json;

namespace Savanna.Core
{
    /// <summary>
    /// Main game engine that manages the simulation state and updates
    /// </summary>
    public class GameEngine
    {
        private List<IAnimal> _animals = new List<IAnimal>();
        private readonly Field _field;
        private Random _random = new Random();
        private readonly IConsoleRenderer _renderer;
        private readonly LifeCycleManager _lifeCycleManager = new();
        private readonly PredatorBehaviorManager _predatorManager = new();
        private readonly FileManager _fileManager;

        public GameEngine(IConsoleRenderer renderer)
        {
            _renderer = renderer;
            _field = new Field(GameConstants.DefaultFieldWidth, GameConstants.DefaultFieldHeight);
            _fileManager = new FileManager(renderer);

            _lifeCycleManager.OnAnimalDeath += (animal) =>
            {
                _animals.Remove(animal);
                _renderer.ShowLog($"{animal.Name} died at position ({animal.Position.X},{animal.Position.Y})", GameConstants.LogDurationLong);
            };

            _lifeCycleManager.OnAnimalBirth += (parent, position) =>
            {
                var offspring = (parent as Animal)?.CreateOffspring(position);
                if (offspring != null)
                {
                    AddAnimal(offspring, false);
                    _renderer.ShowLog($"New {offspring.Name} born at position ({position.X},{position.Y})", GameConstants.LogDurationLong);
                }
            };

            _predatorManager.OnHunt += (predator, prey) =>
            {
                _renderer.ShowLog($"{predator.Name} hunted {prey.Name} at position ({prey.Position.X},{prey.Position.Y})", GameConstants.LogDurationMedium);
            };
        }

        public GameEngine(IConsoleRenderer renderer, int fieldWidth, int fieldHeight)
        {
            _renderer = renderer;
            _field = new Field(fieldWidth, fieldHeight);
            _fileManager = new FileManager(renderer);

            _lifeCycleManager.OnAnimalDeath += (animal) =>
            {
                _animals.Remove(animal);
                _renderer.ShowLog($"{animal.Name} died at position ({animal.Position.X},{animal.Position.Y})", GameConstants.LogDurationLong);
            };
        }

        /// <summary>
        /// Adds an animal to the simulation and assigns it a random position within the field boundaries.
        /// </summary>
        /// <param name="animal">The animal instance to add.</param>
        /// <param name="logSpawn">Whether to log the spawning event (default: true).</param>
        public void AddAnimal(IAnimal animal, bool logSpawn = true)
        {
            if (animal.Position == Position.Null)
            {
                animal.Position = new Position(
                    _random.Next(0, _field.Width),
                    _random.Next(0, _field.Height)
                );
            }
            _animals.Add(animal);

            if (logSpawn)
            {
                _renderer.ShowLog($"{animal.Name} spawned at position ({animal.Position.X},{animal.Position.Y})", GameConstants.LogDurationMedium);
            }
        }

        /// <summary>
        /// Updates the simulation by moving animals and invoking their special actions.
        /// </summary>
        public void Update()
        {
            _animals.RemoveAll(animal => !animal.isAlive);

            var currentAnimals = _animals.ToList();

            foreach (var animal in currentAnimals)
            {
                animal.Move(_animals, _field.Width, _field.Height);
            }

            currentAnimals = _animals.ToList();

            foreach (var animal in currentAnimals)
            {
                animal.SpecialAction(_animals);
            }

            _predatorManager.Update(_animals);
            _lifeCycleManager.Update(_animals, _field.Width, _field.Height);
        }

        /// <summary>
        /// Saves the game state to a file
        /// </summary>
        /// <param name="filePath">Path where the save file will be stored</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool SaveGame(string filePath)
        {
            return _fileManager.SaveGame(filePath, _field, _animals);
        }

        /// <summary>
        /// Saves the game to an automatically generated save location with timestamp
        /// </summary>
        /// <returns>Path to the saved file if successful, empty string otherwise</returns>
        public string SaveGame()
        {
            return _fileManager.SaveGame(_field, _animals);
        }

        /// <summary>
        /// Loads the game from a specified save file
        /// </summary>
        /// <param name="filePath">Path to the save file</param>
        /// <param name="animalFactory">Factory to create animal instances</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool LoadGame(string filePath, IAnimalFactory animalFactory)
        {
            var result = _fileManager.LoadGame(filePath, _field, animalFactory);

            if (result.Success)
            {
                _animals.Clear();
                foreach (var animal in result.Animals)
                {
                    AddAnimal(animal, false);
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if any save files exist
        /// </summary>
        /// <returns>True if at least one save file exists</returns>
        public static bool SaveFilesExist()
        {
            return FileManager.SaveFilesExist();
        }

        /// <summary>
        /// Gets a dictionary of display names mapped to their file paths
        /// </summary>
        /// <returns>Dictionary with display names as keys and file paths as values</returns>
        public static Dictionary<string, string> GetSaveFilesDisplayNames()
        {
            return FileManager.GetSaveFilesDisplayNames();
        }

        /// <summary>
        /// Extracts field dimensions from a save file
        /// </summary>
        /// <param name="filePath">Path to the save file</param>
        /// <param name="width">Output parameter for field width</param>
        /// <param name="height">Output parameter for field height</param>
        /// <returns>True if dimensions were successfully extracted</returns>
        public static bool TryGetSaveFileDimensions(string filePath, out int width, out int height)
        {
            return FileManager.TryGetSaveFileDimensions(filePath, out width, out height);
        }

        /// <summary>
        /// Draws the current state of the field, populating it with animals and rendering via the console.
        /// </summary>
        public void DrawField()
        {
            _field.Clear();
            foreach (var animal in _animals)
            {
                _field.PlaceAnimal(animal);
            }
            _renderer.RenderField(_field.GetGrid());
        }

        /// <summary>
        /// Formats a save filename into a more readable display name
        /// </summary>
        /// <param name="fileName">Raw save filename</param>
        /// <returns>Formatted display name</returns>
        public static string FormatSaveFileDisplayName(string fileName)
        {
            try
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                string[] parts = fileNameWithoutExtension.Split('_');

                if (parts.Length >= 3)
                {
                    string dateStr = parts[1];
                    string timeStr = parts[2];

                    if (dateStr.Length == 8 && timeStr.Length == 6)
                    {
                        string year = dateStr.Substring(0, 4);
                        string month = dateStr.Substring(4, 2);
                        string day = dateStr.Substring(6, 2);

                        string hour = timeStr.Substring(0, 2);
                        string minute = timeStr.Substring(2, 2);
                        string second = timeStr.Substring(4, 2);

                        return $"Save from {year}-{month}-{day} {hour}:{minute}:{second}";
                    }
                }

                return fileName;
            }
            catch
            {
                return fileName;
            }
        }
    }
}
