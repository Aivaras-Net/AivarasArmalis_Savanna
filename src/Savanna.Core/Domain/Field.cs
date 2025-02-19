using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;

public class Field
{
    private readonly char[,] _grid;
    public int Width { get; }
    public int Height { get; }

    public Field(int width, int height)
    {
        Width = width;
        Height = height;
        _grid = new char[height, width];
        Clear();
    }

    public void Clear()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _grid[y, x] = ' ';
            }
        }
    }

    public bool IsInBounds(Position position)
    {
        return position.X >= 0 && position.X < Width &&
               position.Y >= 0 && position.Y < Height;
    }

    public void PlaceAnimal(IAnimal animal)
    {
        if (IsInBounds(animal.Position))
        {
            _grid[animal.Position.Y, animal.Position.X] = animal.Name[0];
        }
    }

    public char[,] GetGrid() => _grid;
}