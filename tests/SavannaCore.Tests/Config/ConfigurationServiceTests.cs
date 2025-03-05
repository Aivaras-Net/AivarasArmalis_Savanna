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
    public void GetAnimalConfig_ShouldCreateNewConfig_WhenAnimalTypeNotFound()
    {
        var testAnimalName = "InvalidAnimal";
        var config = ConfigurationService.GetAnimalConfig(testAnimalName);

        Assert.NotNull(config);
        Assert.True(ConfigurationService.Config.Animals.ContainsKey(testAnimalName));
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
        Assert.Equal(1.0, ConfigurationService.ConfigExtensions.GetHuntingRange(lionConfig));
        Assert.Equal(3, ConfigurationService.ConfigExtensions.GetRoarRange(lionConfig));
        Assert.Equal(0.3, lionConfig.SpecialActionChance);
        Assert.Equal(5.0, ConfigurationService.ConfigExtensions.GetHealthGainFromKill(lionConfig));

        // Antelope settings
        Assert.Equal(1.0, antelopeConfig.Speed);
        Assert.Equal(5.0, antelopeConfig.VisionRange);
        Assert.Equal(0.8, antelopeConfig.SpecialActionChance);
        Assert.Equal(1.0, ConfigurationService.ConfigExtensions.GetHealthFromGrazing(antelopeConfig));
    }
}