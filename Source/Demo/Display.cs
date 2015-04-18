using System;
using System.Diagnostics;
using System.Linq;
using Dungeon.Generator;

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

            try
            {
                var winWidth = Math.Min(width + 7, 132);
                var winHeight = Math.Min(height + 17, 132);

                Console.SetWindowSize(winWidth, winHeight);
            }
            catch
            {
                Debug.Fail("Set console font size to a mono spaced font.");
            }

            Console.SetBufferSize(width + 7, height + 17);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    Console.SetCursorPosition(x + 3, y + 3);
                    var tile = map[x, y];
                    Console.ForegroundColor = ConsoleColor.White;
                    char output;

                    switch (tile.Material)
                    {
                        case TileMaterial.Air:
                            output = ' ';
                            break;
                        case TileMaterial.Wall:
                            Console.ForegroundColor = ConsoleColor.White;
                            output = '\u256C';
                            break;
                        case TileMaterial.Floor:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            output = '\u2591';
                            break;
                        case TileMaterial.BreakableWall:
                            Console.ForegroundColor = ConsoleColor.Green;
                            output = '#';
                            break;

                        default:
                            Console.ForegroundColor = ConsoleColor.Green;
                            output = '?';
                            break;
                    }

                    if (tile.Attributes.HasFlag(TileAttributes.Entry))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        output = 'E';
                    }
                    else if (tile.Attributes.HasFlag(TileAttributes.Exit))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        output = 'E';
                    }
                    else if (tile.Attributes.HasFlag(TileAttributes.Loot))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        output = 'L';
                    }
                    else if (tile.Attributes.HasFlag(TileAttributes.MonsterSpawn))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        output = 'M';
                        
                    }
                    else if (tile.Attributes.HasFlag(TileAttributes.Doors))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        output = 'D';
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

        public bool PromptForInt(string message, out uint seedValue)
        {
            bool valid;
            uint result = 0;
            var startPos = new Point {X = Console.CursorLeft, Y = Console.CursorTop};

            Console.CursorVisible = true;
            var value = "";

            do
            {
                // just clear out the text area each time before we 
                Console.SetCursorPosition(startPos.X, startPos.Y);
                Console.Write(Enumerable.Repeat(' ', message.Length + value.Length).Aggregate("", (s, c) => s + c));
                Console.SetCursorPosition(startPos.X, startPos.Y);
                Console.Write(message);

                 value = Console.ReadLine();
                valid = (value.Equals("q") || value.Equals("cancel") || value.Equals("quit")) || uint.TryParse(value, out result);
                seedValue = result;

            } while (!valid);

            Console.CursorVisible = false;
            return true;
        }

        public void ShowInstructions(uint Seed, MapSize size)
        {

                Console.WriteLine("Seed: {0}", Seed);
                Console.WriteLine("Size: {0}", size);
                Console.WriteLine("Press 'q' to quit");
                Console.WriteLine("Press 'enter' to see a new dungeon");
                Console.WriteLine("Press 'w' to increase dungeon size");
                Console.WriteLine("Press 's' to decrease dungeon size");
                Console.WriteLine("Press 'c' to change the seed");

        }
    }

    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}