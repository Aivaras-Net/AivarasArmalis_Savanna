namespace Savanna.Core.Domain
{
    public interface IAnimal
    {
        string Name { get; }
        double Speed { get; }
        double VisionRange { get; }
        Position Position { get; set; }
        void Move(IEnumerable<IAnimal> animals, int fieldWidth, int fieldHeight);
        void SpecialAction(IEnumerable<IAnimal> animals);
    }
}
