using Savanna.Core.Config;

namespace SavannaCore.Tests.Helpers;

public static class TestConfigHelper
{
    public static AnimalConfig TestConfig { get; } = new()
    {
        Animals = new Dictionary<string, AnimalTypeConfig>
        {
            ["Lion"] = new AnimalTypeConfig
            {
                Speed = 2.0,
                VisionRange = 10.0,
                SpecialActionChance = 0.3,
                Predator = new PredatorConfig
                {
                    HuntingRange = 1.0,
                    RoarRange = 3,
                    HealthGainFromKill = 5.0
                }
            },
            ["Antelope"] = new AnimalTypeConfig
            {
                Speed = 1.0,
                VisionRange = 5.0,
                SpecialActionChance = 0.8,
                Prey = new PreyConfig
                {
                    HealthFromGrazing = 1.0
                }
            }
        },
        General = new GeneralConfig
        {
            InitialHealth = 20.0,
            MaxHealth = 25.0,
            HealthDecreasePerTurn = 0.5,
            RequiredMatingTurns = 3
        }
    };
}