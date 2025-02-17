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
            var animalsList = animals.ToList();
            var target = animalsList.FirstOrDefault(a => a.Name == GameConstants.AntelopeName && a.isAlive && animal.Position.DistanceTo(a.Position) <= 1);

            if (target != null)
            {
                if (animal is Animal predetor && target is Animal prey)
                {
                    predetor.Health = Math.Max(10, predetor.Health + 5);
                    prey.Health = 0;
                    //Console.WriteLine(string.Format(GameConstants.LionSpecialActionMessage, animal.Position, target.Position));
                }
            }
        }
    }
}
