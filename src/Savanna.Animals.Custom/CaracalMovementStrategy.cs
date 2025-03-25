using Savanna.Core.Infrastructure;
using Savanna.Domain;
using Savanna.Domain.Interfaces;

namespace Savanna.Animals.Custom
{
    public class CaracalMovementStrategy : BaseMovementStrategy
    {
        public CaracalMovementStrategy(AnimalConfig config) : base(config)
        {
        }

        public override Position Move(IAnimal animal, IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight)
        {
            //May be replaced with custom animal specific movement logic
            return RandomMove(animal, fieldWidth, fieldHeight);
        }
    }
}