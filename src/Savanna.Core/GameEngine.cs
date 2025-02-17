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
        private readonly List<IAnimal> _animals = new List<IAnimal>();
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
            animal.Position = new Position(_random.Next(0, _fieldWidth), _random.Next(0, _fieldHeight));
            _animals.Add(animal);
        }

        /// <summary>
        /// Updates the simulation by moving animals and invoking their special actions.
        /// </summary>
        public void Update()
        {
            foreach (var animal in _animals)
            {
                animal.Move(_animals, _fieldWidth, _fieldHeight);
            }

            foreach (var animal in _animals)
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
