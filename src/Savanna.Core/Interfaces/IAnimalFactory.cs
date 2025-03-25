using Savanna.Domain.Interfaces;

namespace Savanna.Core.Interfaces
{
    /// <summary>
    /// Interface for a factory that creates animal instances
    /// </summary>
    public interface IAnimalFactory
    {
        /// <summary>
        /// Attempts to create an animal of the specified type
        /// </summary>
        /// <param name="animalType">Type name of the animal to create</param>
        /// <param name="animal">The created animal instance if successful</param>
        /// <returns>True if the animal was created successfully, false otherwise</returns>
        bool TryCreateAnimal(string animalType, out IAnimal animal);
    }
}