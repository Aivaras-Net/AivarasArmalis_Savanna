using Savanna.Core.Interfaces;

namespace Savanna.Web.Services.Interfaces
{
    /// <summary>
    /// Interface for game rendering in web environment
    /// </summary>
    public interface IGameRenderer : IConsoleRenderer
    {
        /// <summary>
        /// Creates a new renderer instance
        /// </summary>
        /// <param name="logAction">Action to call for logging messages</param>
        /// <returns>A new renderer instance</returns>
        IConsoleRenderer CreateRenderer(Action<string> logAction);
    }
}