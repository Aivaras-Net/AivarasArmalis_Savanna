using Savanna.Core.Constants;
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

            if (_random.NextDouble() < GameConstants.LionRoarChance)
            {
                var preyInRoarRange = animals
                    .OfType<IPrey>()
                    .Where(a => a.isAlive && animal.Position.DistanceTo(a.Position) < GameConstants.LionRoarRange);

                foreach (var prey in preyInRoarRange)
                {
                    prey.IsStuned = true;
                }
            }
        }
    }
}
