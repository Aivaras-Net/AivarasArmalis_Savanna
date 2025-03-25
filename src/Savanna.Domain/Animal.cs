using Savanna.Domain.Interfaces;

namespace Savanna.Domain
{
    public abstract class Animal : IAnimal
    {
        protected static IConfigurationProvider _configProvider;

        /// <summary>
        /// Initializes the configuration provider for all animals
        /// </summary>
        /// <param name="configProvider">The configuration provider to use</param>
        public static void InitializeConfigProvider(IConfigurationProvider configProvider)
        {
            _configProvider = configProvider ??
                throw new ArgumentNullException(nameof(configProvider), "Configuration provider cannot be null");
        }

        public abstract string Name { get; }
        public double Health { get; set; }
        public double Speed { get; protected set; }
        public double VisionRange { get; protected set; }
        public Position Position { get; set; }
        public bool isAlive => Health > 0;
        public bool IsStuned { get; set; } = false;
        protected IMovementStrategy MovementStrategy { get; }
        protected ISpecialActionStrategy SpecialActionStrategy { get; }

        protected Animal(double speed, double visionRange, Position position, IAnimalBehavior behavior)
        {
            if (_configProvider == null)
                throw new InvalidOperationException("Configuration provider must be initialized before creating animals. Call Animal.InitializeConfigProvider first.");

            Speed = speed;
            VisionRange = visionRange;
            Position = position;

            var config = _configProvider.Config;
            MovementStrategy = behavior.CreateMovementStrategy(config);
            SpecialActionStrategy = behavior.CreateSpecialActionStrategy(config);
            Health = config.General.InitialHealth;
        }

        public void Move(IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight)
        {
            if (!isAlive) return;
            Position = MovementStrategy.Move(this, animals, fieldWidth, fieldHeight);
            Health -= _configProvider.GeneralConfig.HealthDecreasePerTurn;
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
