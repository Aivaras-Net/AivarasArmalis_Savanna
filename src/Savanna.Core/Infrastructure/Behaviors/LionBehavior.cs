using Savanna.Core.Config;
using Savanna.Core.Constants;
using Savanna.Domain;
using Savanna.Domain.Interfaces;

namespace Savanna.Core.Infrastructure.Behaviors
{
    public class LionBehavior : IAnimalBehavior
    {
        public string AnimalName => GameConstants.LionName;

        public IAnimal CreateAnimal(double speed, double visionRange, Position position)
        {
            return new Lion(speed, visionRange, position);
        }

        public IMovementStrategy CreateMovementStrategy(AnimalConfig config)
        {
            return new LionMovementStrategy(config);
        }

        public ISpecialActionStrategy CreateSpecialActionStrategy(AnimalConfig config)
        {
            return new LionSpecialActionStrategy(config);
        }
    }
}