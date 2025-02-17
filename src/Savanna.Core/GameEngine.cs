using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Interfaces;

namespace Savanna.Core
{
    /// <summary>
    /// Main game engine that manages the simulation state and updates
    /// </summary>
    public class GameEngine
    {
        private List<IAnimal> _animals = new List<IAnimal>();
        private readonly int _fieldWidth;
        private readonly int _fieldHeight;
        private Random _random = new Random();
        private readonly IConsoleRenderer _renderer;

        public GameEngine(IConsoleRenderer renderer)
        {
            _renderer = renderer;
            _fieldWidth = GameConstants.DefaultFieldWidth;
            _fieldHeight = GameConstants.DefaultFieldHeight;
        }

        /// <summary>
        /// Adds an animal to the simulation and assigns it a random position within the field boundaries.
        /// </summary>
        /// <param name="animal">The animal instance to add.</param>
        public void AddAnimal(IAnimal animal)
        {
            if (animal is Animal a)
            {
                a.OnReproduction += HandleReproduction;
            }
            animal.Position = new Position(_random.Next(0, _fieldWidth), _random.Next(0, _fieldHeight));
            _animals.Add(animal);
        }

        private void HandleReproduction(IAnimal parent)
        {
            var possiblePositions = new List<Position>();

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    var newX = parent.Position.X + dx;
                    var newY = parent.Position.Y + dy;

                    if(newX >= 0 && newX < _fieldWidth && newY >= 0 && newY < _fieldHeight && _animals.Any(a => a.Position.X == newX && a.Position.Y == newY))
                    { 
                        possiblePositions.Add(new Position(newX, newY));
                    }

                }
            }

            if (possiblePositions.Count > 0)
            {
                var birthPosition = possiblePositions[_random.Next(possiblePositions.Count)];
                var offspring = (parent as Animal)?.CreateOffspring(birthPosition);
                if (offspring != null)
                {
                    AddAnimal(offspring);
                    //Console.WriteLine($"A new {offspring.Name} was born");
                }
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
                animal.Move(_animals, _fieldWidth, _fieldHeight);
            }

            currentAnimals = _animals.ToList();

            foreach (var animal in currentAnimals)
            {
                animal.SpecialAction(_animals);
            }
        }

        /// <summary>
        /// Draws the current state of the field, populating it with animals and rendering via the console.
        /// </summary>
        public void DrawField()
        {
            char[,] field = new char[_fieldHeight, _fieldWidth];

            for (int y = 0; y < _fieldHeight; y++)
            {
                for (int x = 0; x < _fieldWidth; x++)
                {
                    field[y, x] = ' ';
                }
            }

            foreach (var animal in _animals)
            {
                if ((animal.Position.X >= 0 && animal.Position.X <= _fieldWidth) && (animal.Position.Y >= 0 && animal.Position.Y < _fieldHeight))
                {
                    field[animal.Position.Y, animal.Position.X] = animal.Name[0];
                }
            }

            _renderer.RenderField(field);
        }
    }
}
