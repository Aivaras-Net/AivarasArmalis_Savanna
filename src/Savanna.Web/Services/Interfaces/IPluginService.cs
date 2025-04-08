using Savanna.Domain.Interfaces;

namespace Savanna.Web.Services.Interfaces
{
    /// <summary>
    /// Service for loading and managing plugins
    /// </summary>
    public interface IPluginService
    {
        /// <summary>
        /// Loads plugins from the specified folder
        /// </summary>
        /// <param name="pluginsFolder">The folder containing plugins</param>
        void LoadPlugins(string pluginsFolder);

        /// <summary>
        /// Gets a list of all loaded animal types from plugins
        /// </summary>
        /// <returns>List of animal names</returns>
        IEnumerable<string> GetAvailablePluginAnimals();

        /// <summary>
        /// Attempts to create an animal of the given type
        /// </summary>
        /// <param name="animalType">The type of animal to create</param>
        /// <param name="animal">The created animal if successful</param>
        /// <returns>True if animal was created successfully</returns>
        bool TryCreatePluginAnimal(string animalType, out IAnimal animal);

        /// <summary>
        /// Gets details about a plugin animal
        /// </summary>
        /// <param name="animalType">The type of animal</param>
        /// <returns>Information about the animal</returns>
        PluginAnimalInfo GetPluginAnimalInfo(string animalType);
    }

    /// <summary>
    /// Information about a plugin animal
    /// </summary>
    public class PluginAnimalInfo
    {
        /// <summary>
        /// The name of the animal
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The assembly the animal is defined in
        /// </summary>
        public string SourceAssembly { get; set; } = string.Empty;

        /// <summary>
        /// The animal type
        /// </summary>
        public string AnimalType { get; set; } = string.Empty;

        /// <summary>
        /// Whether the animal is a predator
        /// </summary>
        public bool IsPredator { get; set; }

        /// <summary>
        /// Whether the animal is prey
        /// </summary>
        public bool IsPrey { get; set; }

        /// <summary>
        /// The base speed of the animal
        /// </summary>
        public double Speed { get; set; }

        /// <summary>
        /// The vision range of the animal
        /// </summary>
        public double VisionRange { get; set; }

        /// <summary>
        /// The chance of performing a special action
        /// </summary>
        public double SpecialActionChance { get; set; }

        /// <summary>
        /// The hunting range if the animal is a predator
        /// </summary>
        public double? HuntingRange { get; set; }

        /// <summary>
        /// The amount of health gained from killing prey (predator only)
        /// </summary>
        public double? HealthGainFromKill { get; set; }

        /// <summary>
        /// The roar range if the animal is a predator
        /// </summary>
        public int? RoarRange { get; set; }

        /// <summary>
        /// The amount of health gained from grazing (prey only)
        /// </summary>
        public double? HealthFromGrazing { get; set; }
    }
}