using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using SavannaCore.Tests.Helpers;

namespace SavannaCore.Tests.Domain;

public class AntelopeTests
{
    [Fact]
    public void Antelope_CreateOffspring_ShouldInheritProperties()
    {
        var parentAntelope = AnimalTestHelper.CreateAntelope(AnimalTestHelper.CreatePosition(0, 0));
        var offspringPosition = AnimalTestHelper.CreatePosition(1, 1);

        var offspring = parentAntelope.CreateOffspring(offspringPosition);

        Assert.IsType<Antelope>(offspring);
        Assert.Equal(offspringPosition, offspring.Position);
        Assert.Equal(parentAntelope.Speed, offspring.Speed);
        Assert.Equal(parentAntelope.VisionRange, offspring.VisionRange);
    }

    [Fact]
    public void Antelope_IsStuned_ShouldBeInitiallyFalse()
    {
        var antelope = AnimalTestHelper.CreateAntelope(AnimalTestHelper.CreatePosition(0, 0));

        Assert.False(((IPrey)antelope).IsStuned);
    }
}