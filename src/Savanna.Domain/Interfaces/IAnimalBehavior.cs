namespace Savanna.Domain.Interfaces
{
    public interface IAnimalBehavior
    {
        string AnimalName { get; }
        IAnimal CreateAnimal(double speed, double visionRange, Position position);
        IMovementStrategy CreateMovementStrategy(AnimalConfig config);
        ISpecialActionStrategy CreateSpecialActionStrategy(AnimalConfig config);
    }
}