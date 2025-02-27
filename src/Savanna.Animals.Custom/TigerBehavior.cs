using Savanna.Core.Config;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Interfaces;

namespace Savanna.Animals.Custom
{
    public class TigerBehavior : IAnimalBehavior
    {
        public string AnimalName => "Tiger";

        public IAnimal CreateAnimal(double speed, double visionRange, Position position)
        {
            return new Tiger(speed, visionRange, position);
        }

        public IMovementStrategy CreateMovementStrategy(AnimalConfig config)
        {
            return new TigerMovementStrategy(config);
        }

        public ISpecialActionStrategy CreateSpecialActionStrategy(AnimalConfig config)
        {
            return new TigerSpecialActionStrategy(config);
        }
    }
}