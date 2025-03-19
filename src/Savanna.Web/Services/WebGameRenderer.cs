using Savanna.Core.Config;
using Savanna.Core.Constants;
using Savanna.Core.Interfaces;
using Savanna.Web.Services.Interfaces;
using System.Collections.Generic;

namespace Savanna.Web.Services
{
    /// <summary>
    /// Web implementation of the game renderer
    /// </summary>
    public class WebGameRenderer : IGameRenderer
    {
        private Action<string> _logAction;
        private readonly Dictionary<string, ConsoleColor> _animalColors = new Dictionary<string, ConsoleColor>();

        public WebGameRenderer()
        {
            _logAction = _ => { };

            _animalColors[GameConstants.LionName] = ConsoleColor.Red;
            _animalColors[GameConstants.AntelopeName] = ConsoleColor.Green;

            try
            {
                foreach (var animalType in ConfigurationService.Config.Animals.Keys)
                {
                    if (!_animalColors.ContainsKey(animalType))
                    {
                        RegisterAnimalColor(animalType);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public IConsoleRenderer CreateRenderer(Action<string> logAction)
        {
            _logAction = logAction ?? throw new ArgumentNullException(nameof(logAction));
            return this;
        }

        public void DrawField()
        {
        }

        public ConsoleColor GetAnimalColor(string animalName)
        {
            if (_animalColors.TryGetValue(animalName, out var color))
            {
                return color;
            }

            RegisterAnimalColor(animalName);
            return _animalColors[animalName];
        }

        public void RegisterAnimalColor(string animalName)
        {
            if (!_animalColors.ContainsKey(animalName))
            {
                try
                {
                    var config = ConfigurationService.GetAnimalConfig(animalName);
                    bool isPredator = config.Predator != null;
                    _animalColors[animalName] = isPredator ? ConsoleColor.Red : ConsoleColor.Green;
                }
                catch
                {
                    _animalColors[animalName] = ConsoleColor.Yellow;
                }
            }
        }

        public void RenderField(char[,] grid)
        {
        }

        public void ShowLog(string message, int frames)
        {
            _logAction(message);
        }
    }
}