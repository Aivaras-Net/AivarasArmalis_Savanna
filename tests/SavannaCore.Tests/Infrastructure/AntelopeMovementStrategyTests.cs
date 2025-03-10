using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;
using Savanna.Core.Config;
using SavannaCore.Tests.Helpers;

namespace SavannaCore.Tests.Infrastructure;

public class AntelopeMovementStrategyTests : IDisposable
{
    public AntelopeMovementStrategyTests()
    {
        var testConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_animals.json");
        ConfigurationService.SetConfigPath(testConfigPath);
    }

    public void Dispose()
    {
        ConfigurationService.SetConfigPath(null);
    }

    private class TestAntelopeMovementStrategy : AntelopeMovementStrategy
    {
        public TestAntelopeMovementStrategy() : base(ConfigurationService.Config)
        {
        }
    }

    [Fact]
    public void Move_ShouldFleeFromPredator_WhenPredatorInVisionRange()
    {
        var strategy = new AntelopeMovementStrategy(ConfigurationService.Config);
        var antelope = AnimalTestHelper.CreateAntelope(new Position(5, 5));
        var lion = AnimalTestHelper.CreateLion(new Position(3, 3));
        IAnimal[] animals = { antelope, lion };

        var newPosition = strategy.Move(antelope, animals, 10, 10);

        Assert.True(newPosition.X > 5);
        Assert.True(newPosition.Y > 5);
    }

    [Fact]
    public void Move_ShouldNotMove_WhenStuned()
    {
        var strategy = new TestAntelopeMovementStrategy();
        var antelope = AnimalTestHelper.CreateAntelope(new Position(5, 5));
        ((IPrey)antelope).IsStuned = true;
        IAnimal[] animals = { antelope };

        var newPosition = strategy.Move(antelope, animals, 10, 10);

        Assert.Equal(antelope.Position, newPosition);
        Assert.False(((IPrey)antelope).IsStuned);
    }
}