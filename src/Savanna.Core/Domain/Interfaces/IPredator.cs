namespace Savanna.Core.Domain.Interfaces
{
    public interface IPredator : IAnimal
    {
        //void Hunt(IEnumerable<IPrey> preys);

        double HuntingRange { get; }
    }
}
