using Savanna.Core.Config;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Interfaces;

namespace Savanna.Animals.Custom
{
    public class TigerMovementStrategy : IMovementStrategy
    {
        private readonly AnimalConfig _config;

        public TigerMovementStrategy(AnimalConfig config)
        {
            _config = config;
        }

        public Position Move(IAnimal animal, IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight)
        {
            return animal.Position;
        }
    }
}