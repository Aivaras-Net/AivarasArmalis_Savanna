using Savanna.Core.Config;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Interfaces;

namespace Savanna.Animals.Custom
{
    public class CaracalSpecialActionStrategy : ISpecialActionStrategy
    {
        private readonly AnimalConfig _config;

        public CaracalSpecialActionStrategy(AnimalConfig config)
        {
            _config = config;
        }

        public void Execute(IAnimal animal, IEnumerable<IAnimal> animals)
        {
        }
    }
}