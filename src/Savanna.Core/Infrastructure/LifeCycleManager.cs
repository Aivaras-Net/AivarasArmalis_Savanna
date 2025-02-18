using Savanna.Core.Constants;
using Savanna.Core.Domain;

namespace Savanna.Core.Infrastructure
{
    public class LifeCycleManager
    {
        private readonly Dictionary<IAnimal, int> _matingCounters = new();
        private readonly Dictionary<IAnimal, IAnimal> _potentialMates = new();
        private readonly Random _random = new Random();

        public event Action<IAnimal>? OnAnimalDeath;
        public event Action<IAnimal,Position>? OnAnimalBirth;

        public void Update(IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight)
        {
            var animalsList = animals.ToList();

            foreach (var animal in animalsList)
            {
                if (animal is Animal a)
                {
                    a.Health -= GameConstants.HealthDecresePerTurn;

                    if (!a.isAlive)
                    {
                        OnAnimalDeath.Invoke(a);
                        continue;
                    }
                    HandleMating(a, animalsList,fieldWidth,fieldHeight);
                }
            }
        }

        private void HandleMating(Animal animal, List<IAnimal> animals, int fieldWidth, int fieldHeight)
        {
            var nearbyMate = animals.FirstOrDefault(a =>
                a != animal &&
                a.Name == animal.Name &&
                a.isAlive &&
                animal.Position.DistanceTo(a.Position) <= 1);

            if (_potentialMates.GetValueOrDefault(animal) != nearbyMate)
            {
                _matingCounters[animal] = 0;
                _potentialMates[animal] = nearbyMate;
            }

            if (nearbyMate != null)
            {
                _matingCounters[animal] = _matingCounters.GetValueOrDefault(animal) + 1;
                if (_matingCounters[animal] >= GameConstants.RequiredMatingTurns)
                {
                    if (animal.GetHashCode() < nearbyMate.GetHashCode())
                    {
                        var birthPosition = FindBirthPosition(animal.Position, fieldWidth, fieldHeight,animals);
                        if (birthPosition != null)
                        {
                            OnAnimalBirth?.Invoke(animal, birthPosition);
                        }
                    }
                    _matingCounters[animal] = 0;
                }
            }
        }

        private Position? FindBirthPosition(Position parentPosition, int fieldWidth, int fieldHeight, List<IAnimal> animals)
        {
            var possiblePositions = new List<Position>();

            for (int dx = -1; dx<= 1;  dx++)
            {
                for(int dy = -1; dy<= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    var newX = parentPosition.X + dx;
                    var newY = parentPosition.Y + dy;

                    if(newX >= 0 && newX < fieldWidth && newY >= 0 && newY < fieldHeight && !animals.Any(a => a.Position.X == newX && a.Position.Y == newY))
                    {
                        possiblePositions.Add(new Position(newX, newY));
                    }
                }
            }

            return possiblePositions.Count > 0 ? possiblePositions[_random.Next(possiblePositions.Count)] : null;
        }
    }
}
