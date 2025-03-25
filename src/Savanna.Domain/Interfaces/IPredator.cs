namespace Savanna.Domain.Interfaces
{
    public interface IPredator : IAnimal
    {
        double HuntingRange { get; }
    }
}
