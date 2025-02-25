﻿using Savanna.Core.Config;
using Savanna.Core.Constants;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;

namespace Savanna.Core.Domain
{
    public class Antelope : Animal, IPrey
    {
        public override string Name => GameConstants.AntelopeName;

        public Antelope(double speed, double visionRange, Position position)
            : base(speed, visionRange, position,
                   new AntelopeMovementStrategy(ConfigurationService.Config),
                   new AntelopeSpecialActionStrategy(ConfigurationService.Config))
        {
        }

        public bool IsStuned { get; set; }

        /// <summary>
        /// Creates a new antelope instance at the specified position as offspring.
        /// </summary>
        /// <param name="position">The birth position.</param>
        /// <returns>A new antelope instance.</returns>
        public override IAnimal CreateOffspring(Position position)
        {
            return new Antelope(Speed, VisionRange, position);
        }
    }
}
