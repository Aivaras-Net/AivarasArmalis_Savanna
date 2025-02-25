using Savanna.Core.Config;
using Savanna.Core.Constants;

namespace SavannaCore.Tests.Config;

public class ConfigurationServiceTests
{
    [Fact]
    public void GetAnimalConfig_ShouldReturnCorrectConfig()
    {
        var lionConfig = ConfigurationService.GetAnimalConfig(GameConstants.LionName);

        Assert.NotNull(lionConfig);
        Assert.Equal(2.0, lionConfig.Speed);
        Assert.Equal(10.0, lionConfig.VisionRange);
    }

    [Fact]
    public void GetAnimalConfig_ShouldThrowException_WhenAnimalTypeNotFound()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            ConfigurationService.GetAnimalConfig("InvalidAnimal"));
        Assert.Contains("Configuration not found", exception.Message);
    }

    [Fact]
    public void Config_ShouldLoadValidConfiguration() //May be better to use a file specifically for testing
    {
        var config = ConfigurationService.Config;

        Assert.NotNull(config);
        Assert.NotNull(config.Animals);
        Assert.NotNull(config.General);
        Assert.Contains(GameConstants.LionName, config.Animals.Keys);
        Assert.Contains(GameConstants.AntelopeName, config.Animals.Keys);
    }

    [Fact]
    public void Config_ShouldHaveCorrectGeneralSettings()
    {
        var config = ConfigurationService.Config;

        Assert.Equal(20.0, config.General.InitialHealth);
        Assert.Equal(25.0, config.General.MaxHealth);
        Assert.Equal(0.5, config.General.HealthDecreasePerTurn);
        Assert.Equal(3, config.General.RequiredMatingTurns);
    }

    [Fact]
    public void Config_ShouldHaveCorrectAnimalSettings()
    {
        var config = ConfigurationService.Config;
        var lionConfig = config.Animals[GameConstants.LionName];
        var antelopeConfig = config.Animals[GameConstants.AntelopeName];

        // Lion settings
        Assert.Equal(2.0, lionConfig.Speed);
        Assert.Equal(10.0, lionConfig.VisionRange);
        Assert.Equal(1.0, lionConfig.HuntingRange);
        Assert.Equal(3, lionConfig.RoarRange);
        Assert.Equal(0.3, lionConfig.RoarChance);
        Assert.Equal(5.0, lionConfig.HealthGainFromKill);

        // Antelope settings
        Assert.Equal(1.0, antelopeConfig.Speed);
        Assert.Equal(5.0, antelopeConfig.VisionRange);
        Assert.Equal(0.8, antelopeConfig.GrazeChance);
        Assert.Equal(1.0, antelopeConfig.HealthFromGrazing);
    }
}