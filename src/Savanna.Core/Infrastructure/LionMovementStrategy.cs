using Savanna.Domain;
using Savanna.Domain.Interfaces;

namespace Savanna.Core.Infrastructure
{
    /// <summary>
    /// Implements movement strategy for Lions, pursuing nearby Prey
    /// </summary>
    public class LionMovementStrategy : BaseMovementStrategy
    {
        public LionMovementStrategy(AnimalConfig config) : base(config)
        {
        }

        public override Position Move(IAnimal animal, IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight)
        {
            var nearbyAntelope = animals.OfType<IPrey>()
                .Where(p => animal.Position.DistanceTo(p.Position) <= animal.VisionRange)
                .FirstOrDefault();

            if (nearbyAntelope != null)
            {
                int deltaX = nearbyAntelope.Position.X - animal.Position.X;
                int deltaY = nearbyAntelope.Position.Y - animal.Position.Y;
                int stepX = deltaX == 0 ? 0 : deltaX > 0 ? 1 : -1;
                int stepY = deltaY == 0 ? 0 : deltaY > 0 ? 1 : -1;

                return ClampPosition(
                    animal.Position.X + stepX * (int)animal.Speed,
                    animal.Position.Y + stepY * (int)animal.Speed,
                    fieldWidth,
                    fieldHeight
                );
            }

            return RandomMove(animal, fieldWidth, fieldHeight);
        }
    }
}
