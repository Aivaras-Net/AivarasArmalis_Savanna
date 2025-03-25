using Savanna.Core.Constants;
using Savanna.Domain;
using Savanna.Domain.Interfaces;
using static Savanna.Core.Config.ConfigurationService.ConfigExtensions;

namespace Savanna.Core.Infrastructure
{
    /// <summary>
    /// Defines the special action strategy for Antelope, enabling it to graze.
    /// </summary>
    public class AntelopeSpecialActionStrategy : ISpecialActionStrategy
    {
        private readonly Random _random = new Random();
        private readonly AnimalConfig _config;

        public AntelopeSpecialActionStrategy(AnimalConfig config)
        {
            _config = config;
        }

        protected virtual double GetRandomValue()
        {
            return _random.NextDouble();
        }

        public void Execute(IAnimal animal, IEnumerable<IAnimal> animals)
        {
            if (!(animal is Animal antelope) || !antelope.isAlive)
                return;

            var antelopeConfig = _config.Animals[GameConstants.AntelopeName];

            var nearbyLions = animals.Any(a =>
                a.Name == GameConstants.LionName &&
                a.isAlive &&
                animal.Position.DistanceTo(a.Position) <= animal.VisionRange);

            var specialActionChance = GetSpecialActionChance(antelopeConfig);
            if (!nearbyLions && GetRandomValue() <= specialActionChance)
            {
                var healthFromGrazing = GetHealthFromGrazing(antelopeConfig);
                antelope.Health = Math.Min(_config.General.MaxHealth,
                    antelope.Health + healthFromGrazing);
            }
        }
    }
}
