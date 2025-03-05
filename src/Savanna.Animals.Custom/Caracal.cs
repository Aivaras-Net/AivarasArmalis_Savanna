using Savanna.Core.Config;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Interfaces;
using Savanna.Animals.Custom.Constants;

namespace Savanna.Animals.Custom
{
    public class Caracal : Animal, IPredator, IAnimalConfigProvider
    {
        public override string Name => CaracalConstants.Name;
        public double HuntingRange => ConfigurationService.ConfigExtensions.GetHuntingRange(
            ConfigurationService.GetAnimalConfig(Name),
            CaracalConstants.DefaultValues.HuntingRange);

        public Caracal(Position position)
            : this(ConfigurationService.GetAnimalConfig(CaracalConstants.Name).Speed,
                  ConfigurationService.GetAnimalConfig(CaracalConstants.Name).VisionRange,
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
            return new AnimalTypeConfig
            {
                Speed = CaracalConstants.DefaultValues.Speed,
                VisionRange = CaracalConstants.DefaultValues.VisionRange,
                SpecialActionChance = CaracalConstants.DefaultValues.SpecialActionChance,
                Predator = new PredatorConfig
                {
                    HuntingRange = CaracalConstants.DefaultValues.HuntingRange
                }
            };
        }
    }
}