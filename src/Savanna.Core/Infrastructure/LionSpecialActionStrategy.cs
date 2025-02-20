using Savanna.Core.Config;
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

            var config = ConfigurationService.GetAnimalConfig(GameConstants.LionName);

            if (_random.NextDouble() < config.RoarChance)
            {
                var preyInRoarRange = animals
                    .OfType<IPrey>()
                    .Where(a => a.isAlive && animal.Position.DistanceTo(a.Position) < config.RoarRange);

                foreach (var prey in preyInRoarRange)
                {
                    prey.IsStuned = true;
                }
            }
        }
    }
}
