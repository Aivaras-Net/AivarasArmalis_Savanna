using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;

namespace SavannaCore.Tests.Helpers;

public static class AnimalTestHelper
{
    private static readonly double DefaultHealth = TestConfigHelper.TestConfig.General.InitialHealth;

    public static Lion CreateLion(Position position, double? health = null)
    {
        var config = TestConfigHelper.TestConfig.Animals["Lion"];
        var lion = new Lion(config.Speed, config.VisionRange, position);
        lion.Health = health ?? DefaultHealth;
        return lion;
    }

    public static Antelope CreateAntelope(Position position, double? health = null)
    {
        var config = TestConfigHelper.TestConfig.Animals["Antelope"];
        var antelope = new Antelope(config.Speed, config.VisionRange, position);
        antelope.Health = health ?? DefaultHealth;
        return antelope;
    }

    public static List<IAnimal> CreateAnimalList(params IAnimal[] animals)
    {
        return new List<IAnimal>(animals);
    }

    public static Position CreatePosition(int x, int y)
    {
        return new Position(x, y);
    }
}