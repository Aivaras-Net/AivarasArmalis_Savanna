namespace Savanna.Domain.Interfaces
{
    /// <summary>
    /// Represents an animal in the Savanna simulation
    /// </summary>
    public interface IAnimal
    {
        string Name { get; }
        double Health { get; set; }
        double Speed { get; }
        double VisionRange { get; }
        Position Position { get; set; }
        bool isAlive { get; }

        Guid Id { get; }
        int Age { get; }
        Guid? ParentId { get; }
        List<Guid> OffspringIds { get; }
        bool IsSelected { get; set; }
        bool IsStuned { get; set; }

        /// <summary>
        /// Moves the animal based on its movement strategy and surrounding animals
        /// </summary>
        /// <param name="animals">All animals currently in the field</param>
        /// <param name="fieldWidth">Width of the game field</param>
        /// <param name="fieldHeight">Height of the game field</param>
        void Move(IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight);

        /// <summary>
        /// Executes the animal's special action (if any) based on surrounding animals
        /// </summary>
        /// <param name="animals">All animals currently in the field</param>
        void SpecialAction(IEnumerable<IAnimal> animals);

        /// <summary>
        /// Increments the animal's age
        /// </summary>
        void IncrementAge();

        /// <summary>
        /// Registers a new offspring with this parent
        /// </summary>
        /// <param name="offspringId">The ID of the offspring animal</param>
        void RegisterOffspring(Guid offspringId);

        /// <summary>
        /// Creates an offspring instance at the specified position
        /// </summary>
        /// <param name="position">The birth position</param>
        /// <returns>A new animal instance</returns>
        IAnimal CreateOffspring(Position position);

        /// <summary>
        /// Event triggered when this animal reproduces
        /// </summary>
        event Action<IAnimal>? OnReproduction;
    }
}
