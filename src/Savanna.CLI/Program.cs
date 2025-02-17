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
            Console.WriteLine("Savanna simulation");
            Console.WriteLine("Press A to add ANtelope, Q to quit");


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
                            Console.WriteLine("Antelope added");
                            break;
                        case ConsoleKey.Q:
                            running = false;
                            break;
                    }
                }

                engine.Update();
                engine.DrawField();
                Thread.Sleep(500);
            }
        }
    }
}
