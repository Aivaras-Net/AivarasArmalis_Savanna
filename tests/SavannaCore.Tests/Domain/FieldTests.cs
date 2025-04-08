using Savanna.Core.Constants;
using Savanna.Domain;
using SavannaCore.Tests.Helpers;

namespace SavannaCore.Tests.Domain;

public class FieldTests
{
    [Fact]
    public void Constructor_ShouldCreateEmptyField()
    {
        var field = new Field(5, 5);

        Assert.Equal(5, field.Width);
        Assert.Equal(5, field.Height);
        var grid = field.GetGrid();
        for (int y = 0; y < field.Height; y++)
        {
            for (int x = 0; x < field.Width; x++)
            {
                Assert.Equal(GameConstants.FieldFill, grid[y, x]);
            }
        }
    }

    [Fact]
    public void IsInBounds_ShouldReturnTrue_ForValidPosition()
    {
        var field = new Field(5, 5);
        var position = new Position(2, 2);

        Assert.True(field.IsInBounds(position));
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(5, 0)]
    [InlineData(0, 5)]
    public void IsInBounds_ShouldReturnFalse_ForInvalidPosition(int x, int y)
    {
        var field = new Field(5, 5);
        var position = new Position(x, y);

        Assert.False(field.IsInBounds(position));
    }

    [Fact]
    public void PlaceAnimal_ShouldPlaceAnimalOnGrid()
    {
        var field = new Field(5, 5);
        var lion = AnimalTestHelper.CreateLion(new Position(2, 2));

        field.PlaceAnimal(lion);

        Assert.Equal('L', field.GetGrid()[2, 2]);
    }

    [Fact]
    public void PlaceAnimal_ShouldNotPlace_WhenPositionOutOfBounds()
    {
        var field = new Field(5, 5);
        var lion = AnimalTestHelper.CreateLion(new Position(5, 5));

        field.PlaceAnimal(lion);

        var grid = field.GetGrid();
        Assert.Equal(GameConstants.FieldFill, grid[4, 4]);
    }

    [Fact]
    public void Clear_ShouldResetAllCellsToEmpty()
    {
        var field = new Field(5, 5);
        var lion = AnimalTestHelper.CreateLion(new Position(2, 2));
        var antelope = AnimalTestHelper.CreateAntelope(new Position(3, 3));
        field.PlaceAnimal(lion);
        field.PlaceAnimal(antelope);

        field.Clear();

        var grid = field.GetGrid();
        for (int y = 0; y < field.Height; y++)
        {
            for (int x = 0; x < field.Width; x++)
            {
                Assert.Equal(GameConstants.FieldFill, grid[y, x]);
            }
        }
    }
}