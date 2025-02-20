using Savanna.Core.Config;
using Savanna.Core.Domain.Interfaces;

namespace Savanna.Core.Infrastructure
{
    /// <summary>
    /// Manages core predator behaviors like hunting
    /// </summary>
    public class PredatorBehaviorManager
    {
        /// <summary>
        /// Updates predator behaviors for all predators in the simulation
        /// </summary>
        /// <param name="animals">All animals in the simulation</param>
        public void Update(IEnumerable<IAnimal> animals)
        {
            var predators = animals.OfType<IPredator>().Where(p => p.isAlive);
            var prey = animals.OfType<IPrey>().Where(p => p.isAlive);

            foreach (var predator in predators)
            {
                Hunt(predator, prey);
            }
        }

        /// <summary>
        /// Attempts to hunt prey within range.
        /// </summary>
        /// <param name="preys">The collection of potential prey.</param>
        private void Hunt(IPredator predator, IEnumerable<IPrey> prey)
        {
            var target = prey
                .Where(p => predator.Position.DistanceTo(p.Position) <= predator.HuntingRange)
                .OrderBy(p => predator.Position.DistanceTo(p.Position))
                .FirstOrDefault();

            if (target != null)
            {
                double healthGain = Math.Min(ConfigurationService.Config.General.MaxHealth - predator.Health, target.Health);
                predator.Health += healthGain;
                target.Health = 0;
            }
        }
    }
}