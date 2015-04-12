using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dungeon.Generator;

namespace Demo
{
    public class Program
    {
        private static MapSize[] sizes = Enum.GetValues(typeof (MapSize)).Cast<MapSize>().ToArray();
        private static int selectedSize;

        static uint Seed = 1024u;

        static readonly Display display = new Display();
        static Dictionary<ConsoleKey, Action>  _inputMap = new Dictionary<ConsoleKey, Action>()
        {
            {ConsoleKey.W, IncreaseSize},
            {ConsoleKey.S, DecreaseSize},
            {ConsoleKey.Enter, () => { }},
        };

        static void IncreaseSize()
        {
            selectedSize = (selectedSize + 1)%sizes.Length;
        }

        static void DecreaseSize()
        {

            selectedSize--;
            selectedSize = selectedSize < 0 ? sizes.Length - 1 : selectedSize;
        }

        static void Main()
        {
            while (true)
            {
                var size = sizes[selectedSize];
                var dungeon = Generator.Generate(size, Seed++);
                display.ShowDungeon(dungeon);

                Thread.Sleep(100);
                Console.WriteLine("Seed: {0}", Seed);
                Console.WriteLine("Size: {0}", size);
                Console.WriteLine("Press 'enter' to see a new dungeon");
                Console.WriteLine("Press 'w' to increase dungeon size");
                Console.WriteLine("Press 's' to decrease dungeon size");
                var input = Console.ReadKey();
                Action result;
                if (_inputMap.TryGetValue(input.Key, out result))
                    result();

                Console.Clear();
            }
        }

    }
}
