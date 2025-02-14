using Savanna.Core.Domain;

namespace Savanna.Core.Interfaces
{
    public interface ISpecialActionStrategy
    {
        void Execute(IAnimal animal, IEnumerable<IAnimal> animals);
    }
}
