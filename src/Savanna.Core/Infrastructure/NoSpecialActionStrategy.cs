using Savanna.Core.Domain;
using Savanna.Core.Interfaces;

namespace Savanna.Core.Infrastructure
{
    /// <summary>
    /// Represents a strategy that performs no special action for an animal.
    /// </summary>
    public class NoSpecialActionStrategy : ISpecialActionStrategy
    {
        public void Execute(IAnimal animal, IEnumerable<IAnimal> animals)
        {

        }
    }
}
