using Savanna.Core.Config;
using Savanna.Domain;
using Savanna.Domain.Interfaces;

namespace Savanna.Core.Infrastructure
{
    /// <summary>
    /// Base class for animal movement strategies providing common functionality
    /// </summary>
    public abstract class BaseMovementStrategy : IMovementStrategy
    {
        private readonly Random _random = new Random();
        protected readonly AnimalConfig _config;

        protected BaseMovementStrategy(AnimalConfig config)
        {
            _config = config;
        }

        public abstract Position Move(IAnimal animal, IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight);

        /// <summary>
        /// Generates a random movement within the field boundaries
        /// </summary>
        protected Position RandomMove(IAnimal animal, IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight)
        {
            for (int attempt = 0; attempt < 8; attempt++)
            {
                int dx = _random.Next(-1, 2);
                int dy = _random.Next(-1, 2);

                if (dx == 0 && dy == 0) continue;

                int newX = animal.Position.X + dx;
                int newY = animal.Position.Y + dy;

                if (IsValidPosition(new Position(newX, newY), animals, animal, fieldWidth, fieldHeight))
                {
                    return new Position(newX, newY);
                }
            }

            return animal.Position;
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

        /// <summary>
        /// Checks if a position is valid (within boundaries and not occupied by incompatible animals)
        /// </summary>
        /// <param name="position">The position to check</param>
        /// <param name="animals">All animals in the simulation</param>
        /// <param name="movingAnimal">The animal that is moving (to exclude from collision check)</param>
        /// <param name="fieldWidth">Width of the game field</param>
        /// <param name="fieldHeight">Height of the game field</param>
        /// <returns>True if the position is valid, false otherwise</returns>
        protected bool IsValidPosition(Position position, IEnumerable<IAnimal> animals, IAnimal movingAnimal, int fieldWidth, int fieldHeight)
        {
            if (position.X < 0 || position.X >= fieldWidth || position.Y < 0 || position.Y >= fieldHeight)
            {
                return false;
            }

            var animalsAtPosition = animals.Where(a =>
                a != movingAnimal &&
                a.isAlive &&
                a.Position.X == position.X &&
                a.Position.Y == position.Y).ToList();

            if (!animalsAtPosition.Any())
            {
                return true;
            }

            bool isPredator = movingAnimal is IPredator;
            bool isPrey = movingAnimal is IPrey;

            if (isPredator)
            {
                return animalsAtPosition.All(a => a is IPrey);
            }
            else if (isPrey)
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        protected bool ShouldStayForMating(IAnimal animal, IEnumerable<IAnimal> animals)
        {
            if (animals is Animal a && a.Health < ConfigurationService.Config.General.InitialHealth / 2)
            {
                var nearbyMate = animals.FirstOrDefault(other =>
                    other != animal &&
                    other.Name == animal.Name &&
                    other.isAlive &&
                    animal.Position.DistanceTo(other.Position) <= 1);

                return nearbyMate != null;
            }
            return false;
        }

        protected virtual double GetRandomValue()
        {
            return _random.NextDouble();
        }
    }
}