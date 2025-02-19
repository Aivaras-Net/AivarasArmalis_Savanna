using Savanna.Core.Constants;
using Savanna.Core.Domain;

namespace Savanna.Core.Infrastructure
{
    /// <summary>
    /// Implements movement strategy for Antelopes, fleeing from nearby Lions
    /// </summary>
    public class AntelopeMovementStrategy : BaseMovementStrategy
    {
        public override Position Move(IAnimal animal, IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight)
        {
            if (animal is Animal antelope && antelope.IsStuned)
            {
                antelope.IsStuned = false;
                return antelope.Position;
            }

            var nearbyLion = animals.FirstOrDefault(a =>
                a.Name == GameConstants.LionName &&
                animal.Position.DistanceTo(a.Position) <= animal.VisionRange);

            if (nearbyLion != null)
            {
                int deltaX = animal.Position.X - nearbyLion.Position.X;
                int deltaY = animal.Position.Y - nearbyLion.Position.Y;
                int stepX = deltaX == 0 ? 0 : deltaX > 0 ? 1 : -1;
                int stepY = deltaY == 0 ? 0 : deltaY > 0 ? 1 : -1;

                return ClampPosition(
                    animal.Position.X + stepX * (int)animal.Speed,
                    animal.Position.Y + stepY * (int)animal.Speed,
                    fieldWidth,
                    fieldHeight
                );
            }

            if (ShouldStayForMating(animal, animals))
            {
                return animal.Position;
            }

            return RandomMove(animal, fieldWidth, fieldHeight);
        }
    }
}
