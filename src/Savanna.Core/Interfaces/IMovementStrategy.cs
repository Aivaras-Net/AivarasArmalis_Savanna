using Savanna.Core.Domain;

namespace Savanna.Core.Interfaces
{
    /// <summary>
    /// Defines a strategy for how an animal moves in the field
    /// </summary>
    public interface IMovementStrategy
    {
        /// <summary>
        /// Calculates the next position for an animal based on its surroundings
        /// </summary>
        /// <param name="animal">The animal to move</param>
        /// <param name="animals">All animals in the field</param>
        /// <param name="fieldWidth">Width of the game field</param>
        /// <param name="fieldHeight">Height of the game field</param>
        /// <returns>The new position for the animal</returns>
        Position Move(IAnimal animal, IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight);
    }
}
