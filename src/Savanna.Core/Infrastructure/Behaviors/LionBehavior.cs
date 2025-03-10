using Savanna.Core.Config;
using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Interfaces;

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