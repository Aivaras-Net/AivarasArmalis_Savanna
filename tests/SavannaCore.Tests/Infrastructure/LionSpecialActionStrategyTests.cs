using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;
using SavannaCore.Tests.Helpers;

namespace SavannaCore.Tests.Infrastructure;

public class LionSpecialActionStrategyTests
{
    private class TestLionSpecialActionStrategy : LionSpecialActionStrategy
    {
        public TestLionSpecialActionStrategy() : base(TestConfigHelper.TestConfig)
        {
        }

        protected override double GetRandomValue()
        {
            return 0.0; // Always trigger roar
        }
    }

    [Fact]
    public void Execute_ShouldStunPrey_WhenInRoarRange()
    {
        var strategy = new TestLionSpecialActionStrategy();
        var lion = AnimalTestHelper.CreateLion(new Position(0, 0));
        var antelope = AnimalTestHelper.CreateAntelope(new Position(2, 2));
        IAnimal[] animals = { lion, antelope };

        strategy.Execute(lion, animals);

        Assert.True(((IPrey)antelope).IsStuned);
    }

    [Fact]
    public void Execute_ShouldNotStunPrey_WhenOutOfRoarRange()
    {
        var strategy = new TestLionSpecialActionStrategy();
        var lion = AnimalTestHelper.CreateLion(new Position(0, 0));
        var antelope = AnimalTestHelper.CreateAntelope(new Position(4, 4));
        IAnimal[] animals = { lion, antelope };

        strategy.Execute(lion, animals);

        Assert.False(((IPrey)antelope).IsStuned);
    }
}