using Savanna.Core.Domain;
using Savanna.Core.Interfaces;

namespace Savanna.Core
{
    /// <summary>
    /// Base class for animal movement strategies providing common functionality
    /// </summary>
    public abstract class BaseMovementStrategy : IMovementStrategy
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Calculates the next position for an animal based on its surroundings
        /// </summary>
        /// <param name="animal">The animal to move</param>
        /// <param name="animals">All animals in the field</param>
        /// <param name="fieldWidth">Width of the game field</param>
        /// <param name="fieldHeight">Height of the game field</param>
        /// <returns>The new position for the animal</returns>
        public abstract Position Move(IAnimal animal, IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight);

        /// <summary>
        /// Generates a random movement within the field boundaries
        /// </summary>
        protected Position RandomMove(IAnimal animal, int fieldWidth, int fieldHeight)
        {
            int newX = Math.Max(0, Math.Min(fieldWidth - 1, animal.Position.X + _random.Next(-1, 2)));
            int newY = Math.Max(0, Math.Min(fieldHeight - 1, animal.Position.Y + _random.Next(-1, 2)));
            return new Position(newX, newY);
        }

        /// <summary>
        /// Ensures the position stays within field boundaries
        /// </summary>
        protected Position ClampPosition(int x, int y, int fieldWidth, int fieldHeight)
        {
            int newX = Math.Max(0, Math.Min(fieldWidth - 1, x));
            int newY = Math.Max(0, Math.Min(fieldHeight - 1, y));
            return new Position(newX, newY);
        }
    }
}