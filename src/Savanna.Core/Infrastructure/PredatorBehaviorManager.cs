using Savanna.Core.Config;
using Savanna.Core.Domain.Interfaces;
using static Savanna.Core.Config.ConfigurationService.ConfigExtensions;

namespace Savanna.Core.Infrastructure
{
    /// <summary>
    /// Manages core predator behaviors like hunting
    /// </summary>
    public class PredatorBehaviorManager
    {
        /// <summary>
        /// Event triggered when a predator successfully hunts a prey
        /// </summary>
        public event Action<IPredator, IPrey>? OnHunt;

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
        /// <param name="predator">The predator attempting to hunt</param>
        /// <param name="prey">The collection of potential prey.</param>
        private void Hunt(IPredator predator, IEnumerable<IPrey> prey)
        {
            var target = prey
                .Where(p => predator.Position.DistanceTo(p.Position) <= predator.HuntingRange)
                .OrderBy(p => predator.Position.DistanceTo(p.Position))
                .FirstOrDefault();

            if (target != null)
            {
                var config = ConfigurationService.GetAnimalConfig(predator.Name);
                var healthGain = GetHealthGainFromKill(config, target.Health);

                double actualHealthGain = Math.Min(
                    ConfigurationService.Config.General.MaxHealth - predator.Health,
                    healthGain);

                predator.Health += actualHealthGain;
                target.Health = 0;
                OnHunt?.Invoke(predator, target);
            }
        }
    }
}