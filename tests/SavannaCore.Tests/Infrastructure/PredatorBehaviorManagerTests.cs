using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;
using SavannaCore.Tests.Helpers;

namespace SavannaCore.Tests.Infrastructure;

public class PredatorBehaviorManagerTests
{
    [Fact]
    public void Update_ShouldKillPrey_WhenInHuntingRange()
    {
        var manager = new PredatorBehaviorManager();
        var lion = AnimalTestHelper.CreateLion(new Position(0, 0));
        var antelope = AnimalTestHelper.CreateAntelope(new Position(1, 0));
        IAnimal[] animals = { lion, antelope };

        manager.Update(animals);

        Assert.False(antelope.isAlive);
        Assert.True(lion.Health > 20.0); // Initial health + gain from kill
    }

    [Fact]
    public void Update_ShouldNotKillPrey_WhenOutOfHuntingRange()
    {
        var manager = new PredatorBehaviorManager();
        var lion = AnimalTestHelper.CreateLion(new Position(0, 0));
        var antelope = AnimalTestHelper.CreateAntelope(new Position(2, 2));
        IAnimal[] animals = { lion, antelope };

        manager.Update(animals);

        Assert.True(antelope.isAlive);
    }

    [Fact]
    public void Update_ShouldNotExceedMaxHealth_WhenKillingPrey()
    {
        var manager = new PredatorBehaviorManager();
        var lion = AnimalTestHelper.CreateLion(new Position(0, 0), 24.5); // Near max health
        var antelope = AnimalTestHelper.CreateAntelope(new Position(1, 0));
        IAnimal[] animals = { lion, antelope };

        manager.Update(animals);

        // Assert
        Assert.Equal(25.0, lion.Health);
        Assert.False(antelope.isAlive);
    }
}