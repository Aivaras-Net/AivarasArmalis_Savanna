using Savanna.Core.Config;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;

namespace Savanna.Animals.Custom
{
    public class Tiger : Animal, IPredator
    {
        public override string Name => "Tiger";
        public double HuntingRange => ConfigurationService.GetAnimalConfig(Name).HuntingRange ?? 2.0;

        public Tiger(Position position)
            : this(ConfigurationService.GetAnimalConfig("Tiger").Speed,
                  ConfigurationService.GetAnimalConfig("Tiger").VisionRange,
                  position)
        {
        }

        public Tiger(double speed, double visionRange, Position position)
            : base(speed, visionRange, position, new TigerBehavior())
        {
        }

        public override IAnimal CreateOffspring(Position position)
        {
            return new Tiger(Speed, VisionRange, position);
        }
    }
}