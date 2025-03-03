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

        public GameEngine(IConsoleRenderer renderer)
        {
            _renderer = renderer;
            _field = new Field(GameConstants.DefaultFieldWidth, GameConstants.DefaultFieldHeight);

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
        /// Saves the current game state to a file
        /// </summary>
        /// <param name="filePath">Path where the save file will be stored</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool SaveGame(string filePath)
        {
            try
            {
                var gameState = new GameState
                {
                    FieldWidth = _field.Width,
                    FieldHeight = _field.Height,
                    Animals = _animals.Select(SerializableAnimal.FromAnimal).ToList()
                };

                string jsonString = JsonSerializer.Serialize(gameState, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(filePath, jsonString);
                _renderer.ShowLog(string.Format(GameConstants.GameSavedMessage, filePath), GameConstants.LogDurationMedium);
                return true;
            }
            catch (Exception ex)
            {
                _renderer.ShowLog(string.Format(GameConstants.GameSaveErrorMessage, ex.Message), GameConstants.LogDurationLong);
                return false;
            }
        }

        /// <summary>
        /// Loads the game from a specified save file
        /// </summary>
        /// <param name="filePath">Path to the save file</param>
        /// <param name="animalFactory">Factory to create animal instances</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool LoadGame(string filePath, IAnimalFactory animalFactory)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _renderer.ShowLog(string.Format(GameConstants.SaveFileNotFoundMessage, filePath), GameConstants.LogDurationLong);
                    return false;
                }

                string jsonString = File.ReadAllText(filePath);
                var gameState = JsonSerializer.Deserialize<GameState>(jsonString);

                if (gameState == null)
                {
                    _renderer.ShowLog(GameConstants.InvalidSaveFormatMessage, GameConstants.LogDurationLong);
                    return false;
                }

                if (_field.Width != gameState.FieldWidth || _field.Height != gameState.FieldHeight)
                {
                    _renderer.ShowLog($"Field size mismatch. Save: {gameState.FieldWidth}x{gameState.FieldHeight}, Current: {_field.Width}x{_field.Height}", GameConstants.LogDurationLong);
                    return false;
                }

                _animals.Clear();

                foreach (var savedAnimal in gameState.Animals)
                {
                    if (animalFactory.TryCreateAnimal(savedAnimal.Type, out var animal))
                    {
                        animal.Health = savedAnimal.Health;
                        animal.Position = new Position(savedAnimal.PositionX, savedAnimal.PositionY);
                        AddAnimal(animal, false);
                    }
                    else
                    {
                        _renderer.ShowLog($"Could not create animal of type: {savedAnimal.Type}", GameConstants.LogDurationShort);
                    }
                }

                _renderer.ShowLog(string.Format(GameConstants.GameLoadedMessage, Path.GetFileName(filePath)), GameConstants.LogDurationMedium);
                return true;
            }
            catch (Exception ex)
            {
                _renderer.ShowLog(string.Format(GameConstants.GameLoadErrorMessage, ex.Message), GameConstants.LogDurationLong);
                return false;
            }
        }

        /// <summary>
        /// Saves the game to an automatically generated save location with timestamp
        /// </summary>
        /// <returns>Path to the saved file if successful, empty string otherwise</returns>
        public string SaveGame()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string savesDir = Path.Combine(baseDir, GameConstants.SaveGameDirectory);

            if (!Directory.Exists(savesDir))
            {
                Directory.CreateDirectory(savesDir);
            }

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = string.Format(GameConstants.SaveFilePattern, timestamp);
            string savePath = Path.Combine(savesDir, fileName);

            if (SaveGame(savePath))
            {
                return savePath;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets a list of all available save files
        /// </summary>
        /// <returns>A dictionary with save filenames as keys and full paths as values</returns>
        public static Dictionary<string, string> GetSaveFiles()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string savesDir = Path.Combine(baseDir, GameConstants.SaveGameDirectory);

            Dictionary<string, string> saveFiles = new Dictionary<string, string>();

            if (!Directory.Exists(savesDir))
            {
                return saveFiles;
            }

            foreach (string filePath in Directory.GetFiles(savesDir, $"*{GameConstants.SaveFileExtension}"))
            {
                string fileName = Path.GetFileName(filePath);
                saveFiles[fileName] = filePath;
            }

            return saveFiles;
        }

        /// <summary>
        /// Checks if any save files exist
        /// </summary>
        /// <returns>True if at least one save file exists</returns>
        public static bool SaveFilesExist()
        {
            return GetSaveFiles().Count > 0;
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

        /// <summary>
        /// Gets a dictionary of display names mapped to their file paths
        /// </summary>
        /// <returns>Dictionary with display names as keys and file paths as values</returns>
        public static Dictionary<string, string> GetSaveFilesDisplayNames()
        {
            var saveFiles = GetSaveFiles();
            var displayNames = new Dictionary<string, string>();

            foreach (var file in saveFiles)
            {
                string displayName = FormatSaveFileDisplayName(file.Key);
                displayNames[displayName] = file.Value;
            }

            return displayNames;
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
            width = GameConstants.DefaultFieldWidth;
            height = GameConstants.DefaultFieldHeight;

            try
            {
                if (!File.Exists(filePath))
                {
                    return false;
                }

                string jsonString = File.ReadAllText(filePath);
                var gameState = JsonSerializer.Deserialize<GameState>(jsonString);

                if (gameState == null)
                {
                    return false;
                }

                width = gameState.FieldWidth;
                height = gameState.FieldHeight;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
