using Savanna.Core.Config;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Interfaces;
using Savanna.Animals.Custom.Constants;

namespace Savanna.Animals.Custom
{
    public class CaracalBehavior : IAnimalBehavior
    {
        public string AnimalName => CaracalConstants.Name;

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