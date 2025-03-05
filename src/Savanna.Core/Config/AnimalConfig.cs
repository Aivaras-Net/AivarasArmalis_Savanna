using System.Text.Json.Serialization;

namespace Savanna.Core.Config
{
    /// <summary>
    /// Root configuration class containing all animal and general game settings
    /// </summary>
    public class AnimalConfig
    {
        public Dictionary<string, AnimalTypeConfig> Animals { get; set; } = new();
        public GeneralConfig General { get; set; } = new();
    }

    /// <summary>
    /// Configuration settings specific to each animal type
    /// </summary>
    public class AnimalTypeConfig
    {
        public double Speed { get; set; }
        public double VisionRange { get; set; }

        public double SpecialActionChance { get; set; }

        // Predator-specific properties
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PredatorConfig? Predator { get; set; }

        // Prey-specific properties
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PreyConfig? Prey { get; set; }
    }

    /// <summary>
    /// Configuration settings specific to predator animals
    /// </summary>
    public class PredatorConfig
    {
        public double HuntingRange { get; set; }
        public int? RoarRange { get; set; }
        public double? HealthGainFromKill { get; set; }
    }

    /// <summary>
    /// Configuration settings specific to prey animals
    /// </summary>
    public class PreyConfig
    {
        public double HealthFromGrazing { get; set; }
    }

    /// <summary>
    /// General game settings that apply to all animals
    /// </summary>
    public class GeneralConfig
    {
        public double InitialHealth { get; set; }
        public double MaxHealth { get; set; }
        public double HealthDecreasePerTurn { get; set; }
        public int RequiredMatingTurns { get; set; }
    }
}