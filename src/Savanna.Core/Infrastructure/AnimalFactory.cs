using Savanna.Core.Constants;
using Savanna.Core.Domain;

namespace Savanna.Core
{
    public static class AnimalFactory
    {
        public static IAnimal CreateAnimal(string type, double speed, double visionRange, Position position)
        {
            switch (type)
            {
                case GameConstants.AntelopeName:
                    return new Antelope(speed, visionRange, position);
                case GameConstants.LionName:
                    return new Lion(speed, visionRange, position);
                default:
                    throw new ArgumentException(GameConstants.InvalidAnimalName);
            }
        }
    }
}
