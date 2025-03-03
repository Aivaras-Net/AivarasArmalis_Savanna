using Savanna.Core.Config;
using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure.Behaviors;
using Savanna.Core.Interfaces;

namespace Savanna.Core.Infrastructure
{
    /// <summary>
    /// Factory for creating animal instances
    /// </summary>
    public class AnimalFactory : IAnimalFactory
    {
        private static readonly Dictionary<string, IAnimalBehavior> _behaviors = new();

        /// <summary>
        /// Registers an animal behavior
        /// </summary>
        /// <param name="behavior">The behavior to register</param>
        public static void RegisterBehavior(IAnimalBehavior behavior)
        {
            _behaviors[behavior.AnimalName] = behavior;
        }

        /// <summary>
        /// Static constructor to register default behaviors
        /// </summary>
        static AnimalFactory()
        {
            RegisterBehavior(new LionBehavior());
            RegisterBehavior(new AntelopeBehavior());
        }

        /// <summary>
        /// Attempts to create an animal of the specified type
        /// </summary>
        /// <param name="animalType">Type name of the animal to create</param>
        /// <param name="animal">The created animal instance if successful</param>
        /// <returns>True if the animal was created successfully, false otherwise</returns>
        public bool TryCreateAnimal(string animalType, out IAnimal animal)
        {
            animal = null;

            if (!_behaviors.TryGetValue(animalType, out var behavior))
            {
                return false;
            }

            try
            {
                var config = ConfigurationService.GetAnimalConfig(animalType);
                animal = behavior.CreateAnimal(config.Speed, config.VisionRange, Position.Null);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates an animal of the specified type
        /// </summary>
        /// <param name="animalType">The type of animal to create</param>
        /// <param name="position">The initial position of the animal</param>
        /// <returns>The created animal instance</returns>
        public IAnimal CreateAnimal(string animalType, Position position)
        {
            if (!TryCreateAnimal(animalType, out var animal))
            {
                throw new ArgumentException($"Unknown animal type: {animalType}");
            }

            animal.Position = position;
            return animal;
        }
    }
}
