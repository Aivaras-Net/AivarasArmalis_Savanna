using Savanna.Core.Domain;

namespace Savanna.Core
{
    public static class AnimalFactory
    {
        public static IAnimal CreateAnimal(string type, double speed, double visionRange, Position position)
        {
            switch (type)
            {
                case "Antelope":
                    return new Antelope(speed, visionRange, position);
                case "Lion":
                    return new Lion(speed, visionRange, position);
                default:
                    throw new ArgumentException("Invalid animal type");
            }
        }
    }
}
