using Savanna.Core.Interfaces;

namespace Savanna.Core.Domain
{
    public abstract class Animal : IAnimal
    {
        public abstract string Name { get;}
        public double Speed { get; protected set;}
        public double VisionRange { get; protected set;}
        public Position Position { get; set;}

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
            Position = MovementStrategy.Move(this, animals, fieldWidth, fieldHeight);
        }

        public void SpecialAction(IEnumerable<IAnimal> animals)
        {
            SpecialActionStrategy.Execute(this, animals);
        }
    }
}
