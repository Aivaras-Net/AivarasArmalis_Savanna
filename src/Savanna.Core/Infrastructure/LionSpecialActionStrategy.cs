using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Interfaces;

namespace Savanna.Core.Infrastructure
{
    /// <summary>
    /// Implements a special action strategy for a lion, enabling it to consume an adjacent antelope.
    /// </summary>
    public class LionSpecialActionStrategy : ISpecialActionStrategy
    {
        public void Execute(IAnimal animal, IEnumerable<IAnimal> animals)
        {
            var target = animals.FirstOrDefault(a => a.Name == GameConstants.AntelopeName && animal.Position.DistanceTo(a.Position) <= 1);

            if (target != null)
            {
                Console.WriteLine(string.Format(GameConstants.LionSpecialActionMessage, animal.Position, target.Position));
            }
        }
    }
}
