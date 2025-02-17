using Savanna.Core;
using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Infrastructure;

namespace Savanna.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleRenderer renderer = new ConsoleRenderer(ConsoleConstants.HeaderHeight);
            GameEngine engine = new GameEngine(renderer);

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(ConsoleConstants.Header);
            Console.WriteLine(ConsoleConstants.CommandGuide);


            bool running = true;

            while (running)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.A:
                            var antelope = AnimalFactory.CreateAnimal(GameConstants.AntelopeName, GameConstants.AntelopeSpeed, GameConstants.AntelopeVisionRange, new Position(0, 0));
                            engine.AddAnimal(antelope);
                            //renderer.ShowMessage(ConsoleConstants.AntelopeAditionMessage, ConsoleConstants.MessageDuration);
                            break;
                        case ConsoleKey.L:
                            var lion = AnimalFactory.CreateAnimal(GameConstants.LionName, GameConstants.LionSpeed, GameConstants.LionVisionRange, new Position(0, 0));
                            engine.AddAnimal(lion);
                            //renderer.ShowMessage(ConsoleConstants.LionAdditionMessage,ConsoleConstants.MessageDuration);
                            break;
                        case ConsoleKey.Q:
                            running = false;
                            break;
                    }
                }

                engine.Update();
                engine.DrawField();
                Thread.Sleep(ConsoleConstants.IterationDuration);
            }
        }
    }
}
