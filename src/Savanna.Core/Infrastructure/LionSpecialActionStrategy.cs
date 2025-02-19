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
        private readonly Random _random = new Random();
        public void Execute(IAnimal animal, IEnumerable<IAnimal> animals)
        {
            if (!(animal is Animal lion) || !lion.isAlive)
                return;

            var animalsList = animals.ToList();
            var target = animalsList.Where(a =>
                a.Name == GameConstants.AntelopeName &&
                a.isAlive &&
                animal.Position.DistanceTo(a.Position) <= 1).OrderBy(a => animal.Position.DistanceTo(a.Position))
                .FirstOrDefault();

            if (target is Animal prey)
            {
                double healthGain = Math.Min(GameConstants.MaxHealth - lion.Health, prey.Health);

                lion.Health += healthGain;
                prey.Health = 0;
            }

            if(_random.NextDouble() < 0.3)
            {
                var nearbyAntelopes = animalsList
                    .Where(a => a.Name == GameConstants.AntelopeName &&
                    a.isAlive && animal.Position.DistanceTo(a.Position) < 3);

                foreach(var antelope in nearbyAntelopes)
                {
                    if (animal is Animal stunnedAntelope)
                    {
                        stunnedAntelope.IsStuned = true;
                    }
                }
            }
        }
    }
}
