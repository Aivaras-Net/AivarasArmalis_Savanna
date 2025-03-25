using Savanna.Core.Constants;
using Savanna.Domain;
using Savanna.Domain.Interfaces;

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