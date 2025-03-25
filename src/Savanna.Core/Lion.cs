using Savanna.Core.Constants;
using Savanna.Core.Config;
using Savanna.Core.Infrastructure.Behaviors;
using Savanna.Domain.Interfaces;
using Savanna.Domain;

namespace Savanna.Core
{
    public class Lion : Animal, IPredator
    {
        public override string Name => GameConstants.LionName;
        public double HuntingRange => ConfigurationService.ConfigExtensions.GetHuntingRange(ConfigurationService.GetAnimalConfig(GameConstants.LionName));

        public Lion(double speed, double visionRange, Position position)
            : base(speed, visionRange, position, new LionBehavior())
        {
        }

        private Lion(double speed, double visionRange, Position position, Guid parentId)
            : base(speed, visionRange, position, new LionBehavior(), parentId)
        {
        }

        /// <summary>
        /// Creates a new lion instance at the specified position as offspring.
        /// </summary>
        /// <param name="position">The birth position.</param>
        /// <returns>A new lion instance.</returns>
        public override IAnimal CreateOffspring(Position position)
        {
            var offspring = new Lion(Speed, VisionRange, position, Id);
            this.RegisterOffspring(offspring.Id);
            return offspring;
        }
    }
}
