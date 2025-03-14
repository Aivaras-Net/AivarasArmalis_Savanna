﻿using Savanna.Core.Config;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Interfaces;

namespace Savanna.Core.Domain
{
    public abstract class Animal : IAnimal
    {
        private static readonly GeneralConfig GeneralConfig = ConfigurationService.Config.General;

        public abstract string Name { get; }
        public double Health { get; set; } = GeneralConfig.InitialHealth;
        public double Speed { get; protected set; }
        public double VisionRange { get; protected set; }
        public Position Position { get; set; }
        public bool isAlive => Health > 0;
        public bool IsStuned { get; set; } = false;
        protected IMovementStrategy MovementStrategy { get; }
        protected ISpecialActionStrategy SpecialActionStrategy { get; }

        protected Animal(double speed, double visionRange, Position position, IAnimalBehavior behavior)
        {
            Speed = speed;
            VisionRange = visionRange;
            Position = position;
            MovementStrategy = behavior.CreateMovementStrategy(ConfigurationService.Config);
            SpecialActionStrategy = behavior.CreateSpecialActionStrategy(ConfigurationService.Config);
        }

        public void Move(IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight)
        {
            if (!isAlive) return;
            Position = MovementStrategy.Move(this, animals, fieldWidth, fieldHeight);
            Health -= GeneralConfig.HealthDecreasePerTurn;
        }

        public void SpecialAction(IEnumerable<IAnimal> animals)
        {
            if (!isAlive) return;
            SpecialActionStrategy.Execute(this, animals);
        }

        public event Action<IAnimal>? OnReproduction;

        /// <summary>
        /// Creates an offspring instance at the specified position.
        /// </summary>
        /// <param name="position">The birth position.</param>
        /// <returns>A new IAnimal instance.</returns>
        public abstract IAnimal CreateOffspring(Position position);
    }
}
