using System;
using System.Linq;
using System.Threading;
using Dungeon.Generator.Navigation;
using Dungeon.Generator.Generation;

namespace Demo
{
    public class Program
    {
        static void Main()
        {
            Console.Title = "Dungeon generator";
            Console.CursorVisible = false;

            const MapSize size = MapSize.Tiny;
            var seed = 1024u;
            var generator = new Generator();

            var mapDimensions = size.ToDimensions();
            Console.SetWindowSize(Math.Min(mapDimensions.X + 7, 132), Math.Min(mapDimensions.Y + 17, 132));
            while (true)
            {
                var dungeon = generator.GenerateMap(size, seed++);
                Render(dungeon);
                Thread.Sleep(100);
                Console.WriteLine("Press 'enter' to see a new dungeon");
                Console.ReadLine();
                Console.Clear();
            }
        }

        public static void Render(ITileMap map)
        {
            var width = map.Width;
            var height = map.Height;

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    Console.SetCursorPosition(x + 3, y + 3);
                    var tile = map[x, y];
                    Console.ForegroundColor = ConsoleColor.White;
                    char output;

                    switch (tile)
                    {
                        case 0:
                            output = '\u256C';
                            break;
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            output = '\u2591';
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Green;
                            output = '@';
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            output = '?';
                            break;
                    }

                    Console.Write(output);
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(Enumerable.Repeat('\u2500', width + 7).ToArray());
            Console.WriteLine("Width: {0} Height: {1}", width, height);
            Console.WriteLine();
            Console.WriteLine(Enumerable.Repeat('\u2500', width + 7).ToArray());
        }
    }
}
