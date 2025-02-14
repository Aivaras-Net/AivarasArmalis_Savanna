using Savanna.Core.Domain;

namespace Savanna.Core.Interfaces
{
    public interface IMovementStrategy
    {
        Position Move(IAnimal animal, IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight);
    }
}
