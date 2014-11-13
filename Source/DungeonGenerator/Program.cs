using System;
using System.Linq;
using System.Threading;

namespace DungeonGenerator
{
    public class Program
    {
        private const int WIDTH = 128;
        private const int HEIGHT = 24;

        static void Main(string[] args)
        {
            Console.Title = "Dungeon generator";
            Console.CursorVisible = false;
            Console.SetWindowSize(WIDTH + 7, HEIGHT + 17);

            var gen = new Generator(WIDTH, HEIGHT);
            Render(gen);
            while (true)
            {
                Render(gen);
                Thread.Sleep(100);
                if (!gen.Step())
                {
                    Console.WriteLine("\nSim over, press enter to run a new sim.");
                    Console.ReadLine();
                    Console.Clear();
                    gen = new Generator(128, 24);
                    Render(gen);
                }
            }
            
        }

        public static void Render(Generator generator)
        {
            var dungeon = generator.Dungeon;
            var width = dungeon.Width;
            var height = dungeon.Height;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Console.SetCursorPosition(x + 3, y + 3);
                    var tile = dungeon[x, y];
                    Console.ForegroundColor = ConsoleColor.White;
                    var output = "";

                    switch (tile)
                    {
                        case TileType.Wall:
                            output = "\u256C";
                            break;
                        case TileType.Floor:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            output = "\u2591";
                            break;
                        case TileType.Air:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    Console.Write(output);
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(Enumerable.Repeat('\u2500', width + 7).ToArray());
            Console.WriteLine("Builder Count: {0}\tMax Generation: {1}", generator.BuilderCount, generator.Generation);
            Console.WriteLine();
            Console.WriteLine(Enumerable.Repeat('\u2500', width + 7).ToArray());
        }
    }
}
