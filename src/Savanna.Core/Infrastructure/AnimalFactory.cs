using Savanna.Core.Config;
using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;

namespace Savanna.Core.Infrastructure
{
    /// <summary>
    /// Provides a factory for instantiating animal objects based on the specified type and attributes.
    /// </summary>
    public static class AnimalFactory
    {
        private static readonly Dictionary<string, IAnimalBehavior> _behaviors = new();

        public static void RegisterBehavior(IAnimalBehavior behavior)
        {
            _behaviors[behavior.AnimalName] = behavior;
        }

        /// <summary>
        /// Creates an animal instance with the provided type, speed, vision range, and initial position.
        /// </summary>
        /// <param name="type">The type of the animal</param>
        /// <param name="position">The initial position of the animal in the simulation.</param>
        /// <returns>An instance of IAnimal corresponding to the specified type.</returns>
        /// <exception cref="ArgumentException">Thrown when an invalid animal type is provided.</exception>
        public static IAnimal CreateAnimal(string type, Position position)
        {
            var config = ConfigurationService.GetAnimalConfig(type);

            if (_behaviors.TryGetValue(type, out var behavior))
            {
                return behavior.CreateAnimal(config.Speed, config.VisionRange, position);
            }

            // Fallback to built-in animals
            switch (type)
            {
                case GameConstants.AntelopeName:
                    return new Antelope(config.Speed, config.VisionRange, position);
                case GameConstants.LionName:
                    return new Lion(config.Speed, config.VisionRange, position);
                default:
                    throw new ArgumentException(GameConstants.InvalidAnimalName);
            }
        }

        public static IAnimal CreateAnimal(string type)
        {
            return CreateAnimal(type, Position.Null);
        }
    }
}
