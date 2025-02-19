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
        /// <summary>
        /// Creates an animal instance with the provided type, speed, vision range, and initial position.
        /// </summary>
        /// <param name="type">The type of the animal</param>
        /// <param name="speed">The speed at which the animal moves.</param>
        /// <param name="visionRange">The distance the animal can observe its surroundings.</param>
        /// <param name="position">The initial position of the animal in the simulation.</param>
        /// <returns>An instance of IAnimal corresponding to the specified type.</returns>
        /// <exception cref="ArgumentException">Thrown when an invalid animal type is provided.</exception>
        public static IAnimal CreateAnimal(string type, double speed, double visionRange, Position position)
        {
            switch (type)
            {
                case GameConstants.AntelopeName:
                    return new Antelope(speed, visionRange, position);
                case GameConstants.LionName:
                    return new Lion(speed, visionRange, position);
                default:
                    throw new ArgumentException(GameConstants.InvalidAnimalName);
            }
        }
    }
}
