using Savanna.Domain.Interfaces;

namespace Savanna.Domain
{
    /// <summary>
    /// Represents a serializable game state that can be saved to and loaded from a file
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Width of the game field
        /// </summary>
        public int FieldWidth { get; set; }

        /// <summary>
        /// Height of the game field
        /// </summary>
        public int FieldHeight { get; set; }

        /// <summary>
        /// Collection of serialized animals in the game
        /// </summary>
        public List<SerializableAnimal> Animals { get; set; } = new();
    }

    /// <summary>
    /// Represents an animal in a format that can be serialized
    /// </summary>
    public class SerializableAnimal
    {
        /// <summary>
        /// Type name of the animal (Lion, Antelope, etc.)
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Current health of the animal
        /// </summary>
        public double Health { get; set; }

        /// <summary>
        /// X coordinate of the animal's position
        /// </summary>
        public int PositionX { get; set; }

        /// <summary>
        /// Y coordinate of the animal's position
        /// </summary>
        public int PositionY { get; set; }

        /// <summary>
        /// Speed of the animal
        /// </summary>
        public double Speed { get; set; }

        /// <summary>
        /// Vision range of the animal
        /// </summary>
        public double VisionRange { get; set; }

        /// <summary>
        /// Unique identifier for the animal
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Age of the animal in game ticks
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Reference to the animal's parent, if any
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// References to the animal's offspring
        /// </summary>
        public List<Guid> OffspringIds { get; set; } = new();

        /// <summary>
        /// Creates a serializable animal from an IAnimal instance
        /// </summary>
        public static SerializableAnimal FromAnimal(IAnimal animal)
        {
            return new SerializableAnimal
            {
                Type = animal.Name,
                Health = animal.Health,
                PositionX = animal.Position.X,
                PositionY = animal.Position.Y,
                Speed = animal.Speed,
                VisionRange = animal.VisionRange,
                Id = animal.Id,
                Age = animal.Age,
                ParentId = animal.ParentId,
                OffspringIds = new List<Guid>(animal.OffspringIds)
            };
        }
    }
}