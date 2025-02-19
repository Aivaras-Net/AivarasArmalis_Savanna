using Savanna.Core.Constants;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;

namespace Savanna.Core.Domain
{
    public class Lion : Animal, IPredator
    {
        public override string Name => GameConstants.LionName;
        public double HuntingRange => 1.0;

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

        /// <summary>
        /// Hunts the nearest prey at hunting range.
        /// </summary>
        /// <param name="prey">The collection of potential prey</param>
        public void Hunt(IEnumerable<IPrey> prey)
        {
            var target = prey
                .Where(p => p.isAlive && Position.DistanceTo(p.Position) <= HuntingRange)
                .OrderBy(p => Position.DistanceTo(p.Position))
                .FirstOrDefault();

            if (target != null)
            {
                double healthGain = Math.Min(GameConstants.MaxHealth - Health, target.Health);

                Health += healthGain;
                target.Health = 0;
            }
        }
    }
}
