using System;
using System.Linq;

namespace Demo
{
    public class Display
    {
        public Display()
        {
            Console.Title = "Dungeon Generator";
            Console.CursorVisible = false;
        }

        public void ShowDungeon(ITileMap map)
        {
            var width = map.Width;
            var height = map.Height;

            Console.SetWindowSize(Math.Min(width + 7, 132), Math.Min(height + 17, 132));
            Console.SetBufferSize(width, height);

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