using Savanna.Core.Domain;
using Savanna.Core.Interfaces;

namespace Savanna.Core
{
    public class NoSpecialActionStrategy : ISpecialActionStrategy
    {
        public void Execute(IAnimal animal, IEnumerable<IAnimal> animals)
        {

        }
    }
}
