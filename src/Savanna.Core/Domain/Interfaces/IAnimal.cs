namespace Savanna.Core.Domain.Interfaces
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
    }
}
