using Savanna.Core.Infrastructure;
using Savanna.Core.Config;
using SavannaCore.Tests.Helpers;
using Savanna.Domain;
using Savanna.Domain.Interfaces;

namespace SavannaCore.Tests.Infrastructure;

public class LionSpecialActionStrategyTests : IDisposable
{
    private class TestLionSpecialActionStrategy : LionSpecialActionStrategy
    {
        public TestLionSpecialActionStrategy() : base(ConfigurationService.Config)
        {
        }

        protected override double GetRandomValue()
        {
            return 0.0; // Always trigger roar
        }
    }

    public LionSpecialActionStrategyTests()
    {
        var testConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_animals.json");
        ConfigurationService.SetConfigPath(testConfigPath);
    }

    public void Dispose()
    {
        ConfigurationService.SetConfigPath(null);
    }

    [Fact]
    public void Execute_ShouldStunPrey_WhenInRoarRange()
    {
        var strategy = new TestLionSpecialActionStrategy();
        var lion = AnimalTestHelper.CreateLion(new Position(5, 5));
        var antelope = AnimalTestHelper.CreateAntelope(new Position(6, 6));
        IAnimal[] animals = { lion, antelope };

        strategy.Execute(lion, animals);

        Assert.True(((IPrey)antelope).IsStuned);
    }
}