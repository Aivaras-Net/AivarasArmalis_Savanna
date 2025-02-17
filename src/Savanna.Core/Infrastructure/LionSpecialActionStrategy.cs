using Savanna.Core.Domain;
using Savanna.Core.Interfaces;

namespace Savanna.Core
{
    /// <summary>
    /// Implements a special action strategy for a lion, enabling it to consume an adjacent antelope.
    /// </summary>
    public class LionSpecialActionStrategy : ISpecialActionStrategy
    {
        public void Execute(IAnimal animal, IEnumerable<IAnimal> animals)
        {
            var target = animals.FirstOrDefault(a => a.Name == "Antelope" && animal.Position.DistanceTo(a.Position) <= 1);

            if (target != null)
            {
                Console.WriteLine($"Lion at {animal.Position} eats antelope at {target.Position}.");
            }
        }
    }
}
