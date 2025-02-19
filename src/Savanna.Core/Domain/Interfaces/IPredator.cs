namespace Savanna.Core.Domain.Interfaces
{
    public interface IPredator : IAnimal
    {
        /// <summary>
        /// Attempts to hunt prey within range.
        /// </summary>
        /// <param name="preys">The collection of potential prey.</param>
        void Hunt(IEnumerable<IPrey> preys);

        double HuntingRange {  get; }
    }
}
