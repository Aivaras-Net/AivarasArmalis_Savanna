using Savanna.Core.Config;
using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Interfaces;

namespace Savanna.Core.Infrastructure
{
    /// <summary>
    /// Defines the special action strategy for Antelope, enabling it to graze.
    /// </summary>
    public class AntelopeSpecialActionStrategy : ISpecialActionStrategy
    {
        private readonly Random _random = new Random();

        public void Execute(IAnimal animal, IEnumerable<IAnimal> animals)
        {
            if (!(animal is Animal antelope) || !antelope.isAlive)
                return;

            var config = ConfigurationService.GetAnimalConfig(GameConstants.AntelopeName);

            var nearbyLions = animals.Any(a =>
                a.Name == GameConstants.LionName &&
                a.isAlive &&
                animal.Position.DistanceTo(a.Position) <= animal.VisionRange);

            if (nearbyLions && _random.NextDouble() <= config.GrazeChance)
            {
                antelope.Health = Math.Min(ConfigurationService.Config.General.MaxHealth,
                    antelope.Health + (config.HealthFromGrazing ?? 0));
            }
        }
    }
}
