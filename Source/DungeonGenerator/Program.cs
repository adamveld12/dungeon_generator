using System;

namespace DungeonGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Dungeon generator";
            Console.CursorVisible = false;

            do
            {
                var dungeon = Generator.Generate();
                Render(dungeon);
            } while ( Console.ReadLine() != "q" );
        }

        public static void Render(Dungeon dungeon)
        {
            var width = dungeon.Width;
            var height = dungeon.Height;
            Console.SetWindowSize(width + 7, height + 6);

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

            Console.WriteLine("\n");
        }
    }
}
