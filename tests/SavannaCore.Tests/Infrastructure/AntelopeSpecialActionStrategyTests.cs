using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;
using Savanna.Core.Config;
using SavannaCore.Tests.Helpers;

namespace SavannaCore.Tests.Infrastructure;

public class AntelopeSpecialActionStrategyTests : IDisposable
{
    private readonly double _antelopeHealthFromGrazing = ConfigurationService.ConfigExtensions.GetHealthFromGrazing(
        ConfigurationService.Config.Animals["Antelope"], 1.0);
    private readonly double _antelopeVisionRange = ConfigurationService.Config.Animals["Antelope"].VisionRange;

    public AntelopeSpecialActionStrategyTests()
    {
        var testConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_animals.json");
        ConfigurationService.SetConfigPath(testConfigPath);
    }

    public void Dispose()
    {
        ConfigurationService.SetConfigPath(null);
    }

    private class TestAntelopeSpecialActionStrategy : AntelopeSpecialActionStrategy
    {
        public TestAntelopeSpecialActionStrategy() : base(ConfigurationService.Config)
        {
        }

        protected override double GetRandomValue()
        {
            return 0.0; // Always trigger special action
        }
    }

    [Fact]
    public void Execute_ShouldGraze_WhenNoLionsNearby()
    {
        var strategy = new TestAntelopeSpecialActionStrategy();
        var antelope = AnimalTestHelper.CreateAntelope(new Position(5, 5));
        var maxHealth = ConfigurationService.Config.General.MaxHealth;
        antelope.Health = maxHealth - _antelopeHealthFromGrazing;
        IAnimal[] animals = { antelope };

        strategy.Execute(antelope, animals);

        Assert.Equal(maxHealth, antelope.Health);
    }

    [Fact]
    public void Execute_ShouldNotExceedMaxHealth_WhenGrazing()
    {
        var strategy = new TestAntelopeSpecialActionStrategy();
        var maxHealth = ConfigurationService.Config.General.MaxHealth;
        var antelope = AnimalTestHelper.CreateAntelope(new Position(0, 0), maxHealth - (_antelopeHealthFromGrazing / 2));
        IAnimal[] animals = { antelope };

        strategy.Execute(antelope, animals);

        Assert.True(antelope.Health <= maxHealth);
    }

    [Fact]
    public void Execute_ShouldNotGraze_WhenLionNearby()
    {
        var strategy = new TestAntelopeSpecialActionStrategy();
        var antelope = AnimalTestHelper.CreateAntelope(new Position(0, 0), 10.0);
        var lion = AnimalTestHelper.CreateLion(new Position(0, (int)_antelopeVisionRange - 1));
        IAnimal[] animals = { antelope, lion };
        var initialHealth = antelope.Health;

        strategy.Execute(antelope, animals);

        Assert.Equal(initialHealth, antelope.Health);
    }
}