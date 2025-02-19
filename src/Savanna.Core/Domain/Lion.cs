using Savanna.Core.Constants;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;

namespace Savanna.Core.Domain
{
    public class Lion : Animal, IPredator
    {
        public override string Name => GameConstants.LionName;
        public double HuntingRange => GameConstants.LionHuntingRange;

        public Lion(double speed, double visionRange, Position position) : base(speed, visionRange, position, new LionMovementStrategy(), new LionSpecialActionStrategy())
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
