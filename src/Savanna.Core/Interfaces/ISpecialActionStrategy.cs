using Savanna.Core.Domain.Interfaces;

namespace Savanna.Core.Interfaces
{
    /// <summary>
    /// Defines a strategy for special actions an animal can perform
    /// </summary>
    public interface ISpecialActionStrategy
    {
        /// <summary>
        /// Executes the special action for an animal
        /// </summary>
        /// <param name="animal">The animal performing the action</param>
        /// <param name="animals">All animals in the field</param>
        void Execute(IAnimal animal, IEnumerable<IAnimal> animals);
    }
}
