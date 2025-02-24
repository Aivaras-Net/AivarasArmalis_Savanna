using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Infrastructure;

namespace SavannaCore.Tests.Infrastructure;

public class AnimalFactoryTests
{
    [Fact]
    public void CreateAnimal_Lion_ShouldCreateLionWithCorrectProperties()
    {
        var position = new Position(1, 1);

        var animal = AnimalFactory.CreateAnimal(GameConstants.LionName, position);

        Assert.IsType<Lion>(animal);
        Assert.Equal(position, animal.Position);
        Assert.Equal(2.0, animal.Speed);
        Assert.Equal(10.0, animal.VisionRange);
        Assert.Equal(GameConstants.LionName, animal.Name);
    }

    [Fact]
    public void CreateAnimal_Antelope_ShouldCreateAntelopeWithCorrectProperties()
    {
        var position = new Position(1, 1);

        var animal = AnimalFactory.CreateAnimal(GameConstants.AntelopeName, position);

        Assert.IsType<Antelope>(animal);
        Assert.Equal(position, animal.Position);
        Assert.Equal(1.0, animal.Speed);
        Assert.Equal(5.0, animal.VisionRange);
        Assert.Equal(GameConstants.AntelopeName, animal.Name);
    }

    [Fact]
    public void CreateAnimal_InvalidType_ShouldThrowArgumentException()
    {
        var position = new Position(1, 1);
        var invalidAnimalType = "InvalidAnimal";

        var exception = Assert.Throws<ArgumentException>(() =>
            AnimalFactory.CreateAnimal(invalidAnimalType, position));
    }

    [Fact]
    public void CreateAnimal_WithoutPosition_ShouldCreateAnimalWithNullPosition()
    {
        var animal = AnimalFactory.CreateAnimal(GameConstants.LionName);

        Assert.Equal(Position.Null, animal.Position);
    }
}