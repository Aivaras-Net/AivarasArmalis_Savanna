using Savanna.Core.Config;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;

namespace Savanna.Animals.Custom
{
    public class Caracal : Animal, IPredator
    {
        public override string Name => "Caracal";
        public double HuntingRange => ConfigurationService.GetAnimalConfig(Name).HuntingRange ?? 2.0;

        public Caracal(Position position)
            : this(ConfigurationService.GetAnimalConfig("Caracal").Speed,
                  ConfigurationService.GetAnimalConfig("Caracal").VisionRange,
                  position)
        {
        }

        public Caracal(double speed, double visionRange, Position position)
            : base(speed, visionRange, position, new CaracalBehavior())
        {
        }

        public override IAnimal CreateOffspring(Position position)
        {
            return new Caracal(Speed, VisionRange, position);
        }
    }
}