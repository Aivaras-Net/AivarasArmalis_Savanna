using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;
using Savanna.Core.Interfaces;

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

            _lifeCycleManager.OnAnimalDeath += (animal) => _animals.Remove(animal);
            _lifeCycleManager.OnAnimalBirth += (parent, position) =>
            {
                var offspring = (parent as Animal)?.CreateOffspring(position);
                if (offspring != null)
                {
                    AddAnimal(offspring);
                }
            };
        }

        /// <summary>
        /// Adds an animal to the simulation and assigns it a random position within the field boundaries.
        /// </summary>
        /// <param name="animal">The animal instance to add.</param>
        public void AddAnimal(IAnimal animal)
        {
            if (animal.Position == Position.Null)
            {
                animal.Position = new Position(
                    _random.Next(0, _field.Width),
                    _random.Next(0, _field.Height)
                );
            }
            _animals.Add(animal);
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
