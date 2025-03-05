using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;
using Savanna.Core.Config;
using SavannaCore.Tests.Helpers;

namespace SavannaCore.Tests.Infrastructure;

public class AntelopeSpecialActionStrategyTests
{
    private readonly double _healthFromGrazing = ConfigurationService.ConfigExtensions.GetHealthFromGrazing(
        TestConfigHelper.TestConfig.Animals["Antelope"], 1.0);
    private readonly double _antelopeVisionRange = TestConfigHelper.TestConfig.Animals["Antelope"].VisionRange;

    private class TestAntelopeSpecialActionStrategy : AntelopeSpecialActionStrategy
    {
        public TestAntelopeSpecialActionStrategy() : base(TestConfigHelper.TestConfig)
        {
        }

        protected override double GetRandomValue()
        {
            return 0.0; // Always return 0 to ensure grazing occurs
        }
    }

    [Fact]
    public void Execute_ShouldGraze_WhenNoLionsNearby()
    {
        var strategy = new TestAntelopeSpecialActionStrategy();
        var antelope = AnimalTestHelper.CreateAntelope(new Position(0, 0), 10.0);
        IAnimal[] animals = { antelope };

        strategy.Execute(antelope, animals);

        Assert.True(antelope.Health > 10.0); // Health increased from grazing
    }

    [Fact]
    public void Execute_ShouldNotExceedMaxHealth_WhenGrazing()
    {
        var strategy = new TestAntelopeSpecialActionStrategy();
        var maxHealth = TestConfigHelper.TestConfig.General.MaxHealth;
        var antelope = AnimalTestHelper.CreateAntelope(new Position(0, 0), maxHealth - (_healthFromGrazing / 2));
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