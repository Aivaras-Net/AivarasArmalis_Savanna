using Savanna.Core.Constants;
using Savanna.Core.Interfaces;
using Savanna.Web.Services.Interfaces;

namespace Savanna.Web.Services
{
    /// <summary>
    /// Web implementation of the game renderer
    /// </summary>
    public class WebGameRenderer : IGameRenderer
    {
        private Action<string> _logAction;

        public WebGameRenderer()
        {
            _logAction = _ => { };
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
            return animalName == GameConstants.LionName ? ConsoleColor.Red : ConsoleColor.Green;
        }

        public void RegisterAnimalColor(string animalName)
        {
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