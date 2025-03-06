using Savanna.Core.Config;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Interfaces;
using Savanna.Animals.Custom.Config;
using Savanna.Animals.Custom.Constants;

namespace Savanna.Animals.Custom
{
    public class Caracal : Animal, IPredator, IAnimalConfigProvider
    {
        public override string Name => PluginConstants.CaracalName;

        public double HuntingRange =>
            CaracalConfig.CaracalTypeConfig.Predator?.HuntingRange ??
            ConfigurationService.ConfigExtensions.GetHuntingRange(
                ConfigurationService.GetAnimalConfig(Name));

        public Caracal(Position position)
            : this(ConfigurationService.GetAnimalConfig(PluginConstants.CaracalName).Speed,
                  ConfigurationService.GetAnimalConfig(PluginConstants.CaracalName).VisionRange,
                  position)
        {
        }

        public Caracal(double speed, double visionRange, Position position)
            : base(speed, visionRange, position, new CaracalBehavior())
        {
        }

        public override IAnimal CreateOffspring(Position position)
        {
            return new Caracal(position);
        }

        public string AnimalName => Name;

        public AnimalTypeConfig GetDefaultConfig()
        {
            return CaracalConfig.CaracalTypeConfig;
        }
    }
}