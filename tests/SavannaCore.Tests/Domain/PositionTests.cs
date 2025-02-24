using Savanna.Core.Domain;

namespace SavannaCore.Tests.Domain;

public class PositionTests
{
    [Theory]
    [InlineData(0, 0, 3, 4, 5)] // Pythagorean triple
    [InlineData(1, 1, 4, 5, 5)]
    [InlineData(0, 0, 0, 0, 0)]
    public void DistanceTo_ShouldCalculateCorrectDistance(int x1, int y1, int x2, int y2, double expected)
    {
        var pos1 = new Position(x1, y1);
        var pos2 = new Position(x2, y2);

        var distance = pos1.DistanceTo(pos2);

        Assert.Equal(expected, distance);
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenCoordinatesMatch()
    {
        var pos1 = new Position(1, 2);
        var pos2 = new Position(1, 2);

        Assert.Equal(pos1, pos2);
    }
}