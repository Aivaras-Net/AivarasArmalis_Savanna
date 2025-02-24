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
        private readonly AnimalConfig _config;

        public LionSpecialActionStrategy(AnimalConfig config)
        {
            _config = config;
        }

        protected virtual double GetRandomValue()
        {
            return _random.NextDouble();
        }

        public void Execute(IAnimal animal, IEnumerable<IAnimal> animals)
        {
            if (!(animal is IPredator lion) || !lion.isAlive)
                return;

            var lionConfig = _config.Animals[GameConstants.LionName];

            if (GetRandomValue() < lionConfig.RoarChance)
            {
                var preyInRoarRange = animals
                    .OfType<IPrey>()
                    .Where(a => a.isAlive && animal.Position.DistanceTo(a.Position) < lionConfig.RoarRange);

                foreach (var prey in preyInRoarRange)
                {
                    prey.IsStuned = true;
                }
            }
        }
    }
}
