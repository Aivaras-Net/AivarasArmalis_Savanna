using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Interfaces;

namespace Savanna.Core.Infrastructure
{
    public class AntelopeSpecialActionStrategy : ISpecialActionStrategy
    {
        private readonly Random _random = new Random();

        public void Execute(IAnimal animal, IEnumerable<IAnimal> animals)
        {
            if (!(animal is Animal antelope) || !antelope.isAlive)
                return;

            var nearbyLions = animals.Any(a =>
                a.Name == GameConstants.LionName &&
                a.isAlive &&
                animal.Position.DistanceTo(a.Position) <= 2);

            if (nearbyLions && _random.NextDouble() <= 0.8)
            {
                antelope.Health = Math.Min(GameConstants.MaxHealth, antelope.Health + GameConstants.HealthFromGrazing);
            }
        }
    }
}
