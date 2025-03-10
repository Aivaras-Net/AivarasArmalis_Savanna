using Savanna.CLI.Interfaces;

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
                serviceContainer.GetService<IMenuRenderer>(),
                serviceContainer.GetService<IMenuInteraction>(),
                serviceContainer.GetService<IGameInitializationService>(),
                serviceContainer.GetService<IConsoleWrapper>()
            );

            game.Run();
        }
    }
}
