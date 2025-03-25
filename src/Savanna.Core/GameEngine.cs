using Savanna.Core.Constants;
using Savanna.Core.Infrastructure;
using Savanna.Core.Interfaces;
using Savanna.Domain;
using Savanna.Domain.Interfaces;

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

        /// <summary>
        /// Gets the current list of animals in the simulation
        /// </summary>
        public IReadOnlyList<IAnimal> Animals => _animals.AsReadOnly();

        public GameEngine(IConsoleRenderer renderer)
        {
            _renderer = renderer;
            _field = new Field(GameConstants.DefaultFieldWidth, GameConstants.DefaultFieldHeight);
            _fileManager = new FileManager(renderer);

            _lifeCycleManager.OnAnimalDeath += (animal) =>
            {
                _animals.Remove(animal);
                _renderer.ShowLog(string.Format(GameConstants.AnimalDiedMessage,
                    animal.Name, animal.Position.X, animal.Position.Y),
                    GameConstants.LogDurationLong);
            };

            _lifeCycleManager.OnAnimalBirth += (parent, position) =>
            {
                var offspring = (parent as Animal)?.CreateOffspring(position);
                if (offspring != null)
                {
                    var otherParent = _animals.FirstOrDefault(a =>
                        a != parent &&
                        a.Name == parent.Name &&
                        a.isAlive &&
                        parent.Position.DistanceTo(a.Position) <= GameConstants.AdjacentDistance);

                    if (otherParent != null)
                    {
                        otherParent.RegisterOffspring(offspring.Id);
                    }

                    if (_animals.Any(a =>
                        a.isAlive &&
                        a.Position.X == offspring.Position.X &&
                        a.Position.Y == offspring.Position.Y))
                    {
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            for (int dy = -1; dy <= 1; dy++)
                            {
                                if (dx == 0 && dy == 0) continue;

                                int newX = Math.Min(_field.Width - 1, Math.Max(0, offspring.Position.X + dx));
                                int newY = Math.Min(_field.Height - 1, Math.Max(0, offspring.Position.Y + dy));

                                if (!_animals.Any(a =>
                                    a.isAlive &&
                                    a.Position.X == newX &&
                                    a.Position.Y == newY))
                                {
                                    offspring.Position = new Position(newX, newY);
                                    break;
                                }
                            }
                        }
                    }

                    AddAnimal(offspring, false);
                    _renderer.ShowLog(string.Format(GameConstants.AnimalBornMessage,
                        offspring.Name, offspring.Position.X, offspring.Position.Y),
                        GameConstants.LogDurationLong);
                }
            };

            _predatorManager.OnHunt += (predator, prey) =>
            {
                _renderer.ShowLog(string.Format(GameConstants.AnimalHuntedMessage,
                    predator.Name, prey.Name, prey.Position.X, prey.Position.Y),
                    GameConstants.LogDurationMedium);
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
                _renderer.ShowLog(string.Format(GameConstants.AnimalDiedMessage,
                    animal.Name, animal.Position.X, animal.Position.Y),
                    GameConstants.LogDurationLong);
            };

            _lifeCycleManager.OnAnimalBirth += (parent, position) =>
            {
                var offspring = (parent as Animal)?.CreateOffspring(position);
                if (offspring != null)
                {
                    var otherParent = _animals.FirstOrDefault(a =>
                        a != parent &&
                        a.Name == parent.Name &&
                        a.isAlive &&
                        parent.Position.DistanceTo(a.Position) <= GameConstants.AdjacentDistance);

                    if (otherParent != null)
                    {
                        otherParent.RegisterOffspring(offspring.Id);
                    }

                    if (_animals.Any(a =>
                        a.isAlive &&
                        a.Position.X == offspring.Position.X &&
                        a.Position.Y == offspring.Position.Y))
                    {
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            for (int dy = -1; dy <= 1; dy++)
                            {
                                if (dx == 0 && dy == 0) continue;

                                int newX = Math.Min(_field.Width - 1, Math.Max(0, offspring.Position.X + dx));
                                int newY = Math.Min(_field.Height - 1, Math.Max(0, offspring.Position.Y + dy));

                                if (!_animals.Any(a =>
                                    a.isAlive &&
                                    a.Position.X == newX &&
                                    a.Position.Y == newY))
                                {
                                    offspring.Position = new Position(newX, newY);
                                    break;
                                }
                            }
                        }
                    }

                    AddAnimal(offspring, false);
                    _renderer.ShowLog(string.Format(GameConstants.AnimalBornMessage,
                        offspring.Name, offspring.Position.X, offspring.Position.Y),
                        GameConstants.LogDurationLong);
                }
            };

            _predatorManager.OnHunt += (predator, prey) =>
            {
                _renderer.ShowLog(string.Format(GameConstants.AnimalHuntedMessage,
                    predator.Name, prey.Name, prey.Position.X, prey.Position.Y),
                    GameConstants.LogDurationMedium);
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
                bool positionFound = false;
                int maxAttempts = GameConstants.MaxSpawnAttempts;
                int attempts = 0;

                while (!positionFound && attempts < maxAttempts)
                {
                    int x = _random.Next(0, _field.Width);
                    int y = _random.Next(0, _field.Height);
                    Position position = new Position(x, y);

                    if (!_animals.Any(a => a.isAlive && a.Position.X == x && a.Position.Y == y))
                    {
                        animal.Position = position;
                        positionFound = true;
                    }

                    attempts++;
                }

                if (!positionFound)
                {
                    animal.Position = new Position(
                        _random.Next(0, _field.Width),
                        _random.Next(0, _field.Height)
                    );

                    _renderer.ShowLog(GameConstants.PositionOccupiedWarningMessage,
                        GameConstants.LogDurationShort);
                }
            }
            _animals.Add(animal);

            if (logSpawn)
            {
                _renderer.ShowLog(string.Format(GameConstants.AnimalSpawnedMessage,
                    animal.Name, animal.Position.X, animal.Position.Y),
                    GameConstants.LogDurationMedium);
            }
        }

        /// <summary>
        /// Updates the simulation by moving animals and invoking their special actions.
        /// </summary>
        public void Update()
        {
            _animals.RemoveAll(animal => !animal.isAlive);

            var predators = _animals.Where(a => a is IPredator).ToList();
            var prey = _animals.Where(a => a is IPrey).ToList();
            var otherAnimals = _animals.Where(a => !(a is IPredator) && !(a is IPrey)).ToList();

            // Step 1: Move predators first (they get priority in movement)
            foreach (var predator in predators)
            {
                predator.Move(_animals, _field.Width, _field.Height);
            }

            // Step 2: Check for hunting after predators have moved
            _predatorManager.Update(_animals);

            // Step 3: Remove dead animals 
            _animals.RemoveAll(animal => !animal.isAlive);

            // Step 4: Move prey (those that survived the predator movement phase)
            foreach (var preyAnimal in prey.Where(p => p.isAlive))
            {
                preyAnimal.Move(_animals, _field.Width, _field.Height);
            }

            // Step 5: Move any other animal types
            foreach (var animal in otherAnimals)
            {
                animal.Move(_animals, _field.Width, _field.Height);
            }

            // Step 6: Special actions for all surviving animals
            var currentAnimals = _animals.ToList();
            foreach (var animal in currentAnimals.Where(a => a.isAlive))
            {
                animal.SpecialAction(_animals);
            }

            // Step 7: Life cycle updates (reproduction, aging, natural death)
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
    }
}
