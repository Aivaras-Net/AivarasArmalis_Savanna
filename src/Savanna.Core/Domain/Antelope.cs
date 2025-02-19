using Savanna.Core.Constants;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;

namespace Savanna.Core.Domain
{
    public class Antelope : Animal,IPrey
    {
        public override string Name => GameConstants.AntelopeName;

        public Antelope(double speed, double visionRange, Position position) : base(speed, visionRange, position, new AntelopeMovementStrategy(), new NoSpecialActionStrategy()) {
            {

            }
        }

        public override IAnimal CreateOffspring(Position position)
        {
            return new Antelope(Speed, VisionRange, position);
        }
    }
}
