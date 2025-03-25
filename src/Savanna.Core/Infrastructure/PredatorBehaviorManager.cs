using Savanna.Core.Config;
using Savanna.Core.Constants;
using Savanna.Domain;
using Savanna.Domain.Interfaces;
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
            var overlappingPrey = prey
                .Where(p => p.Position.X == predator.Position.X && p.Position.Y == predator.Position.Y)
                .FirstOrDefault();

            if (overlappingPrey != null)
            {
                var config = ConfigurationService.GetAnimalConfig(predator.Name);
                var healthGain = GetHealthGainFromKill(config, overlappingPrey.Health);

                double actualHealthGain = Math.Min(
                    ConfigurationService.Config.General.MaxHealth - predator.Health,
                    healthGain);

                predator.Health += actualHealthGain;
                overlappingPrey.Health = 0;
                OnHunt?.Invoke(predator, overlappingPrey);
                return;
            }

            var nearbyPrey = prey
                .Where(p => p.Position.DistanceTo(predator.Position) <= predator.HuntingRange)
                .OrderBy(p => p.Position.DistanceTo(predator.Position))
                .FirstOrDefault();

            if (nearbyPrey != null && nearbyPrey is IPrey preyTarget)
            {
                preyTarget.IsStuned = true;
            }
        }
    }
}