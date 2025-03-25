using Savanna.Core.Config;
using Savanna.Domain.Interfaces;

namespace Savanna.Web.Models
{
    /// <summary>
    /// View model for displaying animal details in the web interface
    /// </summary>
    public class AnimalDetailViewModel
    {
        /// <summary>
        /// Unique identifier for the animal
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Type/species name of the animal
        /// </summary>
        public string Species { get; set; } = string.Empty;

        /// <summary>
        /// Current actual health value of the animal
        /// </summary>
        public double Health { get; set; }

        /// <summary>
        /// Maximum possible health for animals
        /// </summary>
        public double MaxHealth { get; set; }

        /// <summary>
        /// Current health percentage of the animal (calculated)
        /// </summary>
        public double HealthPercentage => (Health / MaxHealth) * 100;

        /// <summary>
        /// Current age of the animal (in game ticks)
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Number of offspring this animal has produced
        /// </summary>
        public int OffspringCount { get; set; }

        /// <summary>
        /// Position X coordinate
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Position Y coordinate
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Whether this animal is currently selected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Creates an AnimalDetailViewModel from an IAnimal
        /// </summary>
        /// <param name="animal">The animal to create the view model from</param>
        /// <returns>A new AnimalDetailViewModel instance</returns>
        public static AnimalDetailViewModel FromAnimal(IAnimal animal)
        {
            return new AnimalDetailViewModel
            {
                Id = animal.Id,
                Species = animal.Name,
                Health = animal.Health,
                MaxHealth = ConfigurationService.Config.General.MaxHealth,
                Age = animal.Age,
                OffspringCount = animal.OffspringIds.Count,
                X = animal.Position.X,
                Y = animal.Position.Y,
                IsSelected = animal.IsSelected
            };
        }
    }
}