using Savanna.Core;
using Savanna.Core.Domain;

namespace Savanna.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleRenderer renderer = new ConsoleRenderer(2);
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
                            var antelope = AnimalFactory.CreateAnimal("Antelope", 1, 5, new Position(0, 0));
                            engine.AddAnimal(antelope);
                            renderer.ShowMessage(ConsoleConstants.AntelopeAditionMessage, ConsoleConstants.MessageDuration);
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
