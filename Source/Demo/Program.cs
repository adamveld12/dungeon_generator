using System;
using System.Linq;
using System.Threading;
using Dungeon.Generator.Navigation;
using Dungeon.Generator.Generation;
using Dungeon.Generator.Generation.Generators;

namespace Demo
{
    public class Program
    {
        static void Main()
        {
            Console.Title = "Dungeon generator";
            Console.CursorVisible = false;

            var size = MapSize.Tiny;
            var seed = 1024u;
            var generator = new Generator(new RoomFirstGeneratorStrategy());

            var mapDimensions = MapEditorTools.ToPoint(size);
            Console.SetWindowSize(mapDimensions.X + 7, Math.Min(mapDimensions.Y + 17, 132));
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
                    var output = "";

                    switch (tile)
                    {
                        case 0:
                            output = "\u256C";
                            break;
                        case 1:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            output = "\u2591";
                            break;
                        case 2:
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
            Console.WriteLine("Width: {0} Height: {1}", width, height);
            Console.WriteLine();
            Console.WriteLine(Enumerable.Repeat('\u2500', width + 7).ToArray());
        }
    }
}
