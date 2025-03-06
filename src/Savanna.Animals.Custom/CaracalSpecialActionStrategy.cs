using Savanna.Core.Config;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Interfaces;

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

        public void Execute(IAnimal animal, IEnumerable<IAnimal> animals)
        {
            //Example of how to set up a special action for animals for importing

            /*
            if (!(animal is IPredator caracal) || !caracal.isAlive)
                return;

            var caracalConfig = _config.Animals[PluginConstants.CaracalName];
            var specialActionChance = GetSpecialActionChance(caracalConfig);

            if (GetRandomValue() < specialActionChance)
            {
            }
            */
        }
    }
}