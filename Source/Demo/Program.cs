using System;
using System.Threading;
using Dungeon.Generator;

namespace Demo
{
    public class Program
    {
        static uint Seed = 1024u;
        const MapSize size = MapSize.Small;

        static readonly Display display = new Display();

        static void Main()
        {
            while (true)
            {
                var dungeon = Generator.Generate(size, Seed++);
                display.ShowDungeon(dungeon);

                Thread.Sleep(100);
                Console.WriteLine("Press 'enter' to see a new dungeon");
                Console.ReadLine();
                Console.Clear();
            }
        }

    }
}
