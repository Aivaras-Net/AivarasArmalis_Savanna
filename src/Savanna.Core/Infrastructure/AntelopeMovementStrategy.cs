using Savanna.Core.Constants;
using Savanna.Domain;
using Savanna.Domain.Interfaces;

namespace Savanna.Core.Infrastructure
{
    /// <summary>
    /// Implements movement strategy for Antelopes, fleeing from nearby Lions
    /// </summary>
    public class AntelopeMovementStrategy : BaseMovementStrategy
    {
        public AntelopeMovementStrategy(AnimalConfig config) : base(config)
        {
        }

        public override Position Move(IAnimal animal, IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight)
        {
            if (animal is IPrey prey && prey.IsStuned)
            {
                prey.IsStuned = false;
                return animal.Position;
            }

            var nearbyPredator = animals
                .Where(a =>
                    (a.Name == GameConstants.LionName || a.Name == GameConstants.CaracalName) &&
                    a.isAlive &&
                    animal.Position.DistanceTo(a.Position) <= animal.VisionRange)
                .OrderBy(a => animal.Position.DistanceTo(a.Position))
                .FirstOrDefault();

            if (nearbyPredator != null)
            {
                bool criticalDanger = animal.Position.DistanceTo(nearbyPredator.Position) <= 2;
                int fleeDistance = criticalDanger ? (int)Math.Ceiling(animal.Speed) : (int)animal.Speed;

                int deltaX = animal.Position.X - nearbyPredator.Position.X;
                int deltaY = animal.Position.Y - nearbyPredator.Position.Y;

                int directionX = deltaX == 0 ? 0 : (deltaX > 0 ? 1 : -1);
                int directionY = deltaY == 0 ? 0 : (deltaY > 0 ? 1 : -1);

                int newX = animal.Position.X + directionX * fleeDistance;
                int newY = animal.Position.Y + directionY * fleeDistance;
                var fleePosition = ClampPosition(newX, newY, fieldWidth, fieldHeight);

                if (IsValidPosition(fleePosition, animals, animal, fieldWidth, fieldHeight))
                {
                    return fleePosition;
                }

                Position bestEscapePosition = animal.Position;
                double bestDistanceToPredator = animal.Position.DistanceTo(nearbyPredator.Position);

                int searchRange = criticalDanger ? 2 : 1;

                for (int dx = -searchRange; dx <= searchRange; dx++)
                {
                    for (int dy = -searchRange; dy <= searchRange; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;

                        bool movingAway = (deltaX >= 0 && dx > 0) || (deltaX <= 0 && dx < 0) ||
                                         (deltaY >= 0 && dy > 0) || (deltaY <= 0 && dy < 0);

                        if (!movingAway && criticalDanger)
                            continue; // In critical danger, only move away

                        var alternativePosition = ClampPosition(
                            animal.Position.X + dx,
                            animal.Position.Y + dy,
                            fieldWidth,
                            fieldHeight);

                        double distanceToPredator = alternativePosition.DistanceTo(nearbyPredator.Position);

                        if (distanceToPredator > bestDistanceToPredator &&
                            IsValidPosition(alternativePosition, animals, animal, fieldWidth, fieldHeight))
                        {
                            bestEscapePosition = alternativePosition;
                            bestDistanceToPredator = distanceToPredator;
                        }
                    }
                }

                if (bestDistanceToPredator > animal.Position.DistanceTo(nearbyPredator.Position))
                {
                    return bestEscapePosition;
                }

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

                        if (IsValidPosition(alternativePosition, animals, animal, fieldWidth, fieldHeight))
                        {
                            return alternativePosition;
                        }
                    }
                }
            }

            if (ShouldStayForMating(animal, animals))
            {
                return animal.Position;
            }

            return RandomMove(animal, animals, fieldWidth, fieldHeight);
        }
    }
}

