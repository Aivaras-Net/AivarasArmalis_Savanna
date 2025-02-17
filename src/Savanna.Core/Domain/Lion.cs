using Savanna.Core.Constants;
using Savanna.Core.Infrastructure;

namespace Savanna.Core.Domain
{
    public class Lion : Animal
    {
        public override string Name => GameConstants.LionName;

        public Lion(double speed, double visionRange, Position position) : base(speed, visionRange, position, new LionMovementStrategy(), new LionSpecialActionStrategy())
        {

        }

        public override IAnimal CreateOffspring(Position position)
        {
            return new Lion(Speed, VisionRange, position);
        }
    }
}
