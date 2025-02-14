using Savanna.Core;
using Savanna.Core.Domain;

namespace Savanna.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameEngine engine = new GameEngine();

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
                Console.Clear();
                engine.DrawField();
                Thread.Sleep(500);
            }
        }
    }
}
