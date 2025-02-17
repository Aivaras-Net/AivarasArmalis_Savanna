namespace Savanna.Core.Domain
{
    public class Lion : Animal
    {
        public override string Name => "Lion";

        public Lion(double speed, double visionRange, Position position) : base(speed, visionRange, position, new LionMovementStrategy(), new LionSpecialActionStrategy())
        {

        }
    }
}
