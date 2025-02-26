using Savanna.Core.Constants;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;
using Savanna.Core.Config;
using Savanna.Core.Infrastructure.Behaviors;

namespace Savanna.Core.Domain
{
    public class Lion : Animal, IPredator
    {
        public override string Name => GameConstants.LionName;
        public double HuntingRange => ConfigurationService.GetAnimalConfig(GameConstants.LionName).HuntingRange ?? 1.0;

        public Lion(double speed, double visionRange, Position position)
            : base(speed, visionRange, position, new LionBehavior())
        {
        }

        /// <summary>
        /// Creates a new lion instance at the specified position as offspring.
        /// </summary>
        /// <param name="position">The birth position.</param>
        /// <returns>A new lion instance.</returns>
        public override IAnimal CreateOffspring(Position position)
        {
            return new Lion(Speed, VisionRange, position);
        }
    }
}
