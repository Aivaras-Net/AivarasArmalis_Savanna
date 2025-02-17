using Savanna.Core.Domain;
using Savanna.Core.Interfaces;

namespace Savanna.Core
{
    public abstract class BaseMovementStrategy : IMovementStrategy
    {
        private readonly Random _random = new Random();

        public abstract Position Move(IAnimal animal, IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight);

        protected Position RandomMove(IAnimal animal, int fieldWidth, int fieldHeight)
        {
            int newX = Math.Max(0, Math.Min(fieldWidth - 1, animal.Position.X + _random.Next(-1, 2)));
            int newY = Math.Max(0, Math.Min(fieldHeight - 1, animal.Position.Y + _random.Next(-1, 2)));
            return new Position(newX, newY);
        }

        protected Position ClampPosition(int x, int y, int fieldWidth, int fieldHeight)
        {
            int newX = Math.Max(0, Math.Min(fieldWidth - 1, x));
            int newY = Math.Max(0, Math.Min(fieldHeight - 1, y));
            return new Position(newX, newY);
        }
    }
}