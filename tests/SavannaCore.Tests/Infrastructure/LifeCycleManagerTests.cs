using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;
using SavannaCore.Tests.Helpers;

namespace SavannaCore.Tests.Infrastructure;

public class LifeCycleManagerTests
{
    [Fact]
    public void Update_ShouldDecreaseHealth()
    {
        var manager = new LifeCycleManager();
        var lion = AnimalTestHelper.CreateLion(new Position(0, 0));
        var initialHealth = lion.Health;
        var animals = new List<IAnimal> { lion };

        manager.Update(animals, 10, 10);

        Assert.Equal(initialHealth - 0.5, lion.Health);
    }

    [Fact]
    public void Update_ShouldTriggerDeathEvent_WhenHealthReachesZero()
    {
        var manager = new LifeCycleManager();
        var lion = AnimalTestHelper.CreateLion(new Position(0, 0), 0.4);
        var deathTriggered = false;
        manager.OnAnimalDeath += (animal) => deathTriggered = true;

        manager.Update(new[] { lion }, 10, 10);

        Assert.True(deathTriggered);
        Assert.False(lion.isAlive);
    }

    [Fact]
    public void Update_ShouldTriggerBirth_WhenMatingConditionsMet()
    {
        var manager = new LifeCycleManager();
        var position1 = new Position(0, 0);
        var position2 = new Position(0, 1);
        var lion1 = AnimalTestHelper.CreateLion(position1);
        var lion2 = AnimalTestHelper.CreateLion(position2);
        var birthTriggered = false;
        manager.OnAnimalBirth += (parent, pos) => birthTriggered = true;

        for (int i = 0; i < 4; i++) // Simulate required mating turns
        {
            manager.Update(new[] { lion1, lion2 }, 10, 10);
        }

        Assert.True(birthTriggered);
    }
}