using Savanna.Core.Constants;
using Savanna.Core.Interfaces;

namespace Savanna.Core.Domain
{
    public abstract class Animal : IAnimal
    {
        public abstract string Name { get; }
        public double Health { get; set; } = GameConstants.InitialHealth;
        public double Speed { get; protected set; }
        public double VisionRange { get; protected set; }
        public Position Position { get; set; }
        public bool isAlive => Health > 0;
        public bool IsStuned { get; set; } = false;
        protected IMovementStrategy MovementStrategy { get; }
        protected ISpecialActionStrategy SpecialActionStrategy { get; }

        public Animal(double speed, double visionRange, Position position, IMovementStrategy movementStrategy, ISpecialActionStrategy specialActionStrategy)
        {
            Speed = speed;
            VisionRange = visionRange;
            Position = position;
            MovementStrategy = movementStrategy;
            SpecialActionStrategy = specialActionStrategy;
        }

        public void Move(IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight)
        {
            if (!isAlive) return;
            Position = MovementStrategy.Move(this, animals, fieldWidth, fieldHeight);
            Health -= 0.5;
        }

        public void SpecialAction(IEnumerable<IAnimal> animals)
        {
            if (!isAlive) return;
            SpecialActionStrategy.Execute(this, animals);
        }

        public event Action<IAnimal>? OnReproduction;

        public abstract IAnimal CreateOffspring(Position position);
    }
}
