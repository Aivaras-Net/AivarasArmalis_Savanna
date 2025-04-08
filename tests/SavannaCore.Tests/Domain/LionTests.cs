using Savanna.Core;
using Savanna.Domain.Interfaces;
using SavannaCore.Tests.Helpers;

namespace SavannaCore.Tests.Domain;

public class LionTests
{
    [Fact]
    public void Lion_CreateOffspring_ShouldInheritProperties()
    {
        var parentLion = AnimalTestHelper.CreateLion(AnimalTestHelper.CreatePosition(0, 0));
        var offspringPosition = AnimalTestHelper.CreatePosition(1, 1);

        var offspring = parentLion.CreateOffspring(offspringPosition);

        Assert.IsType<Lion>(offspring);
        Assert.Equal(offspringPosition, offspring.Position);
        Assert.Equal(parentLion.Speed, offspring.Speed);
        Assert.Equal(parentLion.VisionRange, offspring.VisionRange);
    }

    [Fact]
    public void Lion_HuntingRange_ShouldMatchConfig()
    {
        var lion = AnimalTestHelper.CreateLion(AnimalTestHelper.CreatePosition(0, 0));

        Assert.Equal(1.0, ((IPredator)lion).HuntingRange);
    }
}