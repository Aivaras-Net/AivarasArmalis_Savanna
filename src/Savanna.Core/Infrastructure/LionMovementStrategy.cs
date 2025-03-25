using Savanna.Core.Constants;
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
                .OrderBy(p => animal.Position.DistanceTo(p.Position))
                .FirstOrDefault();

            if (nearbyAntelope != null)
            {
                Position preyPosition = nearbyAntelope.Position;

                int deltaX = preyPosition.X - animal.Position.X;
                int deltaY = preyPosition.Y - animal.Position.Y;
                int stepX = deltaX == 0 ? 0 : deltaX > 0 ? 1 : -1;
                int stepY = deltaY == 0 ? 0 : deltaY > 0 ? 1 : -1;

                if (animal.Position.DistanceTo(preyPosition) <= animal.Speed)
                {
                    if (IsValidPosition(preyPosition, animals, animal, fieldWidth, fieldHeight))
                    {
                        return preyPosition; // Move directly to prey position to hunt
                    }
                }

                int moveDistance = (int)Math.Min(animal.Speed, animal.Position.DistanceTo(preyPosition));
                int newX = animal.Position.X + stepX * moveDistance;
                int newY = animal.Position.Y + stepY * moveDistance;
                var chasePosition = ClampPosition(newX, newY, fieldWidth, fieldHeight);

                if (IsValidPosition(chasePosition, animals, animal, fieldWidth, fieldHeight))
                {
                    return chasePosition;
                }

                Position bestAlternative = animal.Position;
                double bestDistance = animal.Position.DistanceTo(preyPosition);

                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;

                        var alternativePosition = ClampPosition(
                            animal.Position.X + dx,
                            animal.Position.Y + dy,
                            fieldWidth,
                            fieldHeight);

                        double distanceToPrey = alternativePosition.DistanceTo(preyPosition);

                        if (distanceToPrey < bestDistance &&
                            IsValidPosition(alternativePosition, animals, animal, fieldWidth, fieldHeight))
                        {
                            bestAlternative = alternativePosition;
                            bestDistance = distanceToPrey;
                        }
                    }
                }

                if (bestDistance < animal.Position.DistanceTo(preyPosition))
                {
                    return bestAlternative;
                }
            }

            return RandomMove(animal, animals, fieldWidth, fieldHeight);
        }
    }
}
