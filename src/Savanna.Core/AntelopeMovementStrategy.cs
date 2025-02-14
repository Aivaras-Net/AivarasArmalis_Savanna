using Savanna.Core.Domain;
using Savanna.Core.Interfaces;

namespace Savanna.Core
{
    public class AntelopeMovementStrategy : IMovementStrategy
    {
        public Position Move(IAnimal animal, IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight)
        {
            var nearbyLion = animals.FirstOrDefault(a => a.Name == "Lion" && animal.Position.DistanceTo(a.Position) <= animal.VisionRange);

            if (nearbyLion != null)
            {
                int deltaX = animal.Position.X - nearbyLion.Position.X;
                int deltaY = animal.Position.Y - nearbyLion.Position.Y;
                int stepX = deltaX == 0 ? 0 : deltaX > 0 ? 1 : -1;
                int stepY = deltaY == 0 ? 0 : deltaY > 0 ? 1 : -1;
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
