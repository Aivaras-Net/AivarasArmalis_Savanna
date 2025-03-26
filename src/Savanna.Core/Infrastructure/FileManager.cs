using System.Text.Json;
using Savanna.Core.Constants;
using Savanna.Core.Interfaces;
using Savanna.Domain;
using Savanna.Domain.Interfaces;

namespace Savanna.Core.Infrastructure
{
    /// <summary>
    /// Manages file operations for saving and loading games
    /// </summary>
    public class FileManager
    {
        private readonly IConsoleRenderer _renderer;

        public FileManager(IConsoleRenderer renderer)
        {
            _renderer = renderer;
        }

        /// <summary>
        /// Saves the game state to a file
        /// </summary>
        /// <param name="filePath">Path where the save file will be stored</param>
        /// <param name="field">The game field</param>
        /// <param name="animals">The list of animals</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool SaveGame(string filePath, Field field, IEnumerable<IAnimal> animals)
        {
            try
            {
                var gameState = new GameState
                {
                    FieldWidth = field.Width,
                    FieldHeight = field.Height,
                    Animals = animals.Select(SerializableAnimal.FromAnimal).ToList()
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
        /// Saves the game to an automatically generated save location with timestamp
        /// </summary>
        /// <param name="field">The game field</param>
        /// <param name="animals">The list of animals</param>
        /// <returns>Path to the saved file if successful, empty string otherwise</returns>
        public string SaveGame(Field field, IEnumerable<IAnimal> animals)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string savesDir = Path.Combine(baseDir, GameConstants.SaveGameDirectory);

            if (!Directory.Exists(savesDir))
            {
                Directory.CreateDirectory(savesDir);
            }

            string timestamp = DateTime.Now.ToString(GameConstants.DateTimeFormat);
            string fileName = string.Format(GameConstants.SaveFilePattern, timestamp);
            string savePath = Path.Combine(savesDir, fileName);

            if (SaveGame(savePath, field, animals))
            {
                return savePath;
            }

            return string.Empty;
        }

        /// <summary>
        /// Loads a game state from a file
        /// </summary>
        /// <param name="filePath">Path to the save file</param>
        /// <param name="field">The game field to verify dimensions</param>
        /// <param name="animalFactory">Factory to create animal instances</param>
        /// <returns>Tuple containing success flag and loaded animals if successful</returns>
        public (bool Success, List<IAnimal> Animals) LoadGame(string filePath, Field field, IAnimalFactory animalFactory)
        {
            List<IAnimal> loadedAnimals = new List<IAnimal>();

            try
            {
                if (!File.Exists(filePath))
                {
                    _renderer.ShowLog(string.Format(GameConstants.SaveFileNotFoundMessage, filePath), GameConstants.LogDurationLong);
                    return (false, loadedAnimals);
                }

                string jsonString = File.ReadAllText(filePath);
                var gameState = JsonSerializer.Deserialize<GameState>(jsonString);

                if (gameState == null)
                {
                    _renderer.ShowLog(GameConstants.InvalidSaveFormatMessage, GameConstants.LogDurationLong);
                    return (false, loadedAnimals);
                }

                if (field.Width != gameState.FieldWidth || field.Height != gameState.FieldHeight)
                {
                    _renderer.ShowLog(string.Format(GameConstants.FieldSizeMismatchMessage,
                        gameState.FieldWidth, gameState.FieldHeight, field.Width, field.Height),
                        GameConstants.LogDurationLong);
                    return (false, loadedAnimals);
                }

                foreach (var savedAnimal in gameState.Animals)
                {
                    if (animalFactory.TryCreateAnimal(savedAnimal.Type, out var animal))
                    {
                        animal.Health = savedAnimal.Health;
                        animal.Position = new Position(savedAnimal.PositionX, savedAnimal.PositionY);

                        try
                        {
                            if (animal is Animal a)
                            {
                                typeof(Animal).GetProperty("Id")?.SetValue(a, savedAnimal.Id != default ? savedAnimal.Id : Guid.NewGuid());

                                for (int i = 0; i < savedAnimal.Age; i++)
                                {
                                    animal.IncrementAge();
                                }

                                if (savedAnimal.ParentId.HasValue)
                                {
                                    typeof(Animal).GetProperty("ParentId")?.SetValue(a, savedAnimal.ParentId);
                                }

                                foreach (var offspringId in savedAnimal.OffspringIds)
                                {
                                    a.RegisterOffspring(offspringId);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _renderer.ShowLog(string.Format(GameConstants.PropertySetWarningMessage,
                                                            savedAnimal.Type, ex.Message), GameConstants.LogDurationShort);
                        }

                        loadedAnimals.Add(animal);
                    }
                    else
                    {
                        _renderer.ShowLog(string.Format(GameConstants.CouldNotCreateAnimalMessage,
                            savedAnimal.Type), GameConstants.LogDurationShort);
                    }
                }

                _renderer.ShowLog(string.Format(GameConstants.GameLoadedMessage, Path.GetFileName(filePath)), GameConstants.LogDurationMedium);
                return (true, loadedAnimals);
            }
            catch (Exception ex)
            {
                _renderer.ShowLog(string.Format(GameConstants.GameLoadErrorMessage, ex.Message), GameConstants.LogDurationLong);
                return (false, loadedAnimals);
            }
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

                        return string.Format(GameConstants.SaveFromDateTimeFormat,
                            year, month, day, hour, minute, second);
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