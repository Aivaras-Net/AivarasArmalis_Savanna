using Savanna.Core.Domain;
using Savanna.Core.Interfaces;

namespace Savanna.Core
{
    public class LionMovementStrategy : IMovementStrategy
    {
        public Position Move(IAnimal animal, IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight)
        {
            var nearbyAntelope = animals.FirstOrDefault(a => a.Name == "Antelope" && animal.Position.DistanceTo(a.Position) <= animal.VisionRange);

            if (nearbyAntelope != null)
            {
                int deltaX = nearbyAntelope.Position.X - animal.Position.X;
                int deltaY = nearbyAntelope.Position.Y - animal.Position.Y;
                int stepX = deltaX == 0 ? 0 : (deltaX > 0 ? 1 : -1);
                int stepY = deltaY == 0 ? 0 : (deltaY > 0 ? 1 : -1);
                int newX = Math.Max(0, Math.Min(fieldWidth - 1, animal.Position.X + stepX * (int)animal.Speed));
                int newY = Math.Max(0, Math.Min(fieldHeight - 1, animal.Position.Y + stepY * (int)animal.Speed));
                return new Position(newX, newY);
            }
            else
            {
                Random rand = new Random();
                int newX = Math.Max(0, Math.Min(fieldWidth - 1, animal.Position.X + rand.Next(-1, 2)));
                int newY = Math.Max(0, Math.Min(fieldHeight - 1, animal.Position.Y + rand.Next(-1, 2)));
                return new Position(newX, newY);
            }
        }
    }
}
