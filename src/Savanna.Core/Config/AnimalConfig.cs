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
        public double? HuntingRange { get; set; }
        public int? RoarRange { get; set; }
        public double? RoarChance { get; set; }
        public double? HealthGainFromKill { get; set; }
        public double? GrazeChance { get; set; }
        public double? HealthFromGrazing { get; set; }
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