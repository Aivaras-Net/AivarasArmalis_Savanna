using Savanna.Animals.Custom.Constants;
using Savanna.Domain.Interfaces;
using Savanna.Domain;

namespace Savanna.Animals.Custom
{
    public class CaracalBehavior : IAnimalBehavior
    {
        public string AnimalName => PluginConstants.CaracalName;

        public IAnimal CreateAnimal(double speed, double visionRange, Position position)
        {
            return new Caracal(speed, visionRange, position);
        }

        public IMovementStrategy CreateMovementStrategy(AnimalConfig config)
        {
            return new CaracalMovementStrategy(config);
        }

        public ISpecialActionStrategy CreateSpecialActionStrategy(AnimalConfig config)
        {
            return new CaracalSpecialActionStrategy(config);
        }
    }
}