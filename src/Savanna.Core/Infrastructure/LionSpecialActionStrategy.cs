using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Interfaces;

namespace Savanna.Core.Infrastructure
{
    /// <summary>
    /// Implements a special action strategy for a lion, enabling it to roar.
    /// </summary>
    public class LionSpecialActionStrategy : ISpecialActionStrategy
    {
        private readonly Random _random = new Random();
        public void Execute(IAnimal animal, IEnumerable<IAnimal> animals)
        {
            if (!(animal is IPredator lion) || !lion.isAlive)
                return;

            if(_random.NextDouble() < 0.3)
            {
                var nearbyPreys = animals
                    .OfType<IPrey>()
                    .Where(a =>a.isAlive && animal.Position.DistanceTo(a.Position) < 3);

                foreach(var prey in nearbyPreys)
                {
                    prey.IsStuned = true;
                }
            }
        }
    }
}
