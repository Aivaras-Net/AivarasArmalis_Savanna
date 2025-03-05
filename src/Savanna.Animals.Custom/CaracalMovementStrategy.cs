using Savanna.Core.Config;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;

namespace Savanna.Animals.Custom
{
    public class CaracalMovementStrategy : BaseMovementStrategy
    {
        public CaracalMovementStrategy(AnimalConfig config) : base(config)
        {
        }

        public override Position Move(IAnimal animal, IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight)
        {
            return RandomMove(animal, fieldWidth, fieldHeight);
        }
    }
}