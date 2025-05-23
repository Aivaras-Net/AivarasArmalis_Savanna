using Savanna.Core.Infrastructure;
using Savanna.Core.Config;
using SavannaCore.Tests.Helpers;
using Savanna.Domain;
using Savanna.Domain.Interfaces;

namespace SavannaCore.Tests.Infrastructure;

public class LionMovementStrategyTests : IDisposable
{
    public LionMovementStrategyTests()
    {
        var testConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_animals.json");
        ConfigurationService.SetConfigPath(testConfigPath);
    }

    public void Dispose()
    {
        ConfigurationService.SetConfigPath(null);
    }

    [Fact]
    public void Move_ShouldMoveTowardsPrey_WhenPreyInVisionRange()
    {
        var strategy = new LionMovementStrategy(ConfigurationService.Config);
        var lion = AnimalTestHelper.CreateLion(new Position(0, 0));
        var antelope = AnimalTestHelper.CreateAntelope(new Position(5, 5));
        IAnimal[] animals = { lion, antelope };

        var newPosition = strategy.Move(lion, animals, 10, 10);

        Assert.Equal(2, newPosition.X);
        Assert.Equal(2, newPosition.Y);
    }

    [Fact]
    public void Move_ShouldStayWithinBounds()
    {
        var strategy = new LionMovementStrategy(ConfigurationService.Config);
        var lion = AnimalTestHelper.CreateLion(new Position(9, 9));
        var antelope = AnimalTestHelper.CreateAntelope(new Position(11, 11));
        IAnimal[] animals = { lion, antelope };

        var newPosition = strategy.Move(lion, animals, 10, 10);

        Assert.True(newPosition.X < 10);
        Assert.True(newPosition.Y < 10);
    }
}