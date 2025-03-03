using Savanna.CLI.Interfaces;
using Savanna.Core.Interfaces;

namespace Savanna.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            var serviceContainer = new ServiceContainer();
            serviceContainer.RegisterServices();

            var game = new Game(
                serviceContainer.GetService<IRendererService>(),
                serviceContainer.GetService<IMenuService>(),
                serviceContainer.GetService<IConsoleRenderer>(),
                serviceContainer.GetService<ILogService>()
            );

            game.Run();
        }
    }
}
