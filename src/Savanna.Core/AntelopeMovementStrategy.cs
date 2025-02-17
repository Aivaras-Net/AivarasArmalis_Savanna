﻿using Savanna.Core.Domain;

namespace Savanna.Core
{
    public class AntelopeMovementStrategy : BaseMovementStrategy
    {
        public override Position Move(IAnimal animal, IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight)
        {
            var nearbyLion = animals.FirstOrDefault(a =>
                a.Name == "Lion" &&
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

            return RandomMove(animal, fieldWidth, fieldHeight);
        }
    }
}
