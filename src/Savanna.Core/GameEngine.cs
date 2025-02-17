using Savanna.Core.Domain;
using Savanna.Core.Interfaces;

namespace Savanna.Core
{
    public class GameEngine
    {
        private readonly List<IAnimal> _animals = new List<IAnimal>();
        private readonly int _fieldWidth = 40;
        private readonly int _fieldHeight = 20;
        private Random _random = new Random();
        private IConsoleRenderer renderer;

        public GameEngine(IConsoleRenderer renderer)
        {
            this.renderer = renderer;
        }

        public void AddAnimal(IAnimal animal)
        {
            animal.Position = new Position(_random.Next(0, _fieldWidth), _random.Next(0, _fieldHeight));
            _animals.Add(animal);
        }

        public void Update()
        {
            foreach (var animal in _animals)
            {
                animal.Move(_animals, _fieldWidth, _fieldHeight);
            }

            foreach (var animal in _animals)
            {
                animal.SpecialAction(_animals);
            }
        }


        public void DrawField()
        {
            char[,] field = new char[_fieldHeight, _fieldWidth];

            for (int y = 0; y < _fieldHeight; y++)
            {
                for (int x = 0; x < _fieldWidth; x++)
                {
                    field[y, x] = ' ';
                }
            }

            foreach (var animal in _animals)
            {
                if ((animal.Position.X >= 0 && animal.Position.X <= _fieldWidth) && (animal.Position.Y >= 0 && animal.Position.Y < _fieldHeight))
                {
                    field[animal.Position.Y, animal.Position.X] = animal.Name[0];
                }
            }

            renderer.RenderField(field);
        }
    }
}
