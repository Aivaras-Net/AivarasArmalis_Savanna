using Savanna.Core.Constants;
using Savanna.Core.Infrastructure.Behaviors;
using Savanna.Domain.Interfaces;

namespace Savanna.Domain
{
    public class Antelope : Animal, IPrey
    {
        public override string Name => GameConstants.AntelopeName;

        public Antelope(double speed, double visionRange, Position position)
            : base(speed, visionRange, position, new AntelopeBehavior())
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
            var offspring = new Antelope(Speed, VisionRange, position);
            this.RegisterOffspring(offspring.Id);
            return offspring;
        }
    }
}
