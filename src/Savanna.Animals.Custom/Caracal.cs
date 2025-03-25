using Savanna.Core.Config;
using Savanna.Animals.Custom.Constants;
using Savanna.Domain;
using Savanna.Domain.Interfaces;

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
        private Caracal(double speed, double visionRange, Position position, Guid parentId)
            : base(speed, visionRange, position, new CaracalBehavior(), parentId)
        {
            _config = ConfigurationService.GetAnimalConfig(Name);
        }

        public override IAnimal CreateOffspring(Position position)
        {
            var offspring = new Caracal(Speed, VisionRange, position, Id);
            this.RegisterOffspring(offspring.Id);
            return offspring;
        }
    }
}