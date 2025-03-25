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