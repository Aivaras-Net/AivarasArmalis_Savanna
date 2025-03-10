using Savanna.Core.Config;
using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Interfaces;

namespace Savanna.Core.Infrastructure.Behaviors
{
    public class AntelopeBehavior : IAnimalBehavior
    {
        public string AnimalName => GameConstants.AntelopeName;

        public IAnimal CreateAnimal(double speed, double visionRange, Position position)
        {
            return new Antelope(speed, visionRange, position);
        }

        public IMovementStrategy CreateMovementStrategy(AnimalConfig config)
        {
            return new AntelopeMovementStrategy(config);
        }

        public ISpecialActionStrategy CreateSpecialActionStrategy(AnimalConfig config)
        {
            return new AntelopeSpecialActionStrategy(config);
        }
    }
}