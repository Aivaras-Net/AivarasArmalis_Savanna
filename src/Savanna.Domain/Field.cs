using Savanna.Domain;
using Savanna.Domain.Interfaces;

/// <summary>
/// represents the simulation field where animals reside.
/// </summary>
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

    /// <summary>
    /// Clear the field grid by resetting all cells to field fill charachter.
    /// </summary>
    public void Clear()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _grid[y, x] = DomainConstants.FieldFill;
            }
        }
    }

    /// <summary>
    /// Determines whether the given position is within the boundaries of the field.
    /// </summary>
    /// <param name="position">The position to evaluate</param>
    /// <returns>True if position is within bounds; otherwise, false.</returns>
    public bool IsInBounds(Position position)
    {
        return position.X >= 0 && position.X < Width &&
               position.Y >= 0 && position.Y < Height;
    }

    /// <summary>
    /// Places an animal on the field grid at its current position.
    /// </summary>
    /// <param name="animal"></param>
    public void PlaceAnimal(IAnimal animal)
    {
        if (IsInBounds(animal.Position))
        {
            _grid[animal.Position.Y, animal.Position.X] = animal.Name[0];
        }
    }

    public char[,] GetGrid() => _grid;
}