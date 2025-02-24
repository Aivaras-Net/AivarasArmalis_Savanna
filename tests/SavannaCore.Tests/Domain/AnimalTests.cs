using SavannaCore.Tests.Helpers;

namespace SavannaCore.Tests.Domain;

public class AnimalTests
{
    [Fact]
    public void Animal_InitialHealth_ShouldMatchConfig()
    {
        var lion = AnimalTestHelper.CreateLion(AnimalTestHelper.CreatePosition(0, 0));
        var antelope = AnimalTestHelper.CreateAntelope(AnimalTestHelper.CreatePosition(0, 0));

        Assert.Equal(20.0, lion.Health);
        Assert.Equal(20.0, antelope.Health);
    }

    [Fact]
    public void Animal_IsAlive_ShouldBeFalseWhenHealthIsZero()
    {
        var lion = AnimalTestHelper.CreateLion(AnimalTestHelper.CreatePosition(0, 0), 0);

        Assert.False(lion.isAlive);
    }

    [Fact]
    public void Animal_Move_ShouldDecreaseHealth()
    {
        var lion = AnimalTestHelper.CreateLion(AnimalTestHelper.CreatePosition(0, 0));
        var initialHealth = lion.Health;
        var animals = AnimalTestHelper.CreateAnimalList(lion);

        lion.Move(animals, 10, 10);

        Assert.Equal(initialHealth - 0.5, lion.Health);
    }

    [Fact]
    public void Animal_Move_ShouldNotMoveWhenDead()
    {
        var position = AnimalTestHelper.CreatePosition(0, 0);
        var lion = AnimalTestHelper.CreateLion(position, 0);
        var animals = AnimalTestHelper.CreateAnimalList(lion);

        lion.Move(animals, 10, 10);

        Assert.Equal(position, lion.Position);
    }
}