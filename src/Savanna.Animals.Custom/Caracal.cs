using Savanna.Core.Config;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Animals.Custom.Constants;

namespace Savanna.Animals.Custom
{
    public class Caracal : Animal, IPredator
    {
        private readonly AnimalTypeConfig _config;

        public override string Name => PluginConstants.CaracalName;

        public double HuntingRange =>
            _config.Predator?.HuntingRange ??
            ConfigurationService.ConfigExtensions.GetHuntingRange(
                ConfigurationService.GetAnimalConfig(Name));

        public Caracal(Position position)
            : this(ConfigurationService.GetAnimalConfig(PluginConstants.CaracalName).Speed,
                  ConfigurationService.GetAnimalConfig(PluginConstants.CaracalName).VisionRange,
                  position)
        {
            _config = ConfigurationService.GetAnimalConfig(Name);
        }

        public Caracal(double speed, double visionRange, Position position)
            : base(speed, visionRange, position, new CaracalBehavior())
        {
            _config = ConfigurationService.GetAnimalConfig(Name);
        }

        public override IAnimal CreateOffspring(Position position)
        {
            return new Caracal(position);
        }
    }
}