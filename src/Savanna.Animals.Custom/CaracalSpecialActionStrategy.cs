using Savanna.Core.Config;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Interfaces;
using Savanna.Animals.Custom.Constants;
using static Savanna.Core.Config.ConfigurationService.ConfigExtensions;

namespace Savanna.Animals.Custom
{
    public class CaracalSpecialActionStrategy : ISpecialActionStrategy
    {
        private readonly Random _random = new Random();
        private readonly AnimalConfig _config;

        public CaracalSpecialActionStrategy(AnimalConfig config)
        {
            _config = config;
        }

        protected virtual double GetRandomValue()
        {
            return _random.NextDouble();
        }

        public void Execute(IAnimal animal, IEnumerable<IAnimal> animals)
        {
            if (!(animal is IPredator caracal) || !caracal.isAlive)
                return;

            var caracalConfig = _config.Animals[CaracalConstants.Name];
            var specialActionChance = GetSpecialActionChance(caracalConfig);

            if (GetRandomValue() < specialActionChance)
            {
            }
        }
    }
}