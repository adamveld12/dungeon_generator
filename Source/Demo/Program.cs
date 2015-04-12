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
            {ConsoleKey.C, ChangeSeed},
            {ConsoleKey.Q, Quit},

            {ConsoleKey.Enter, () => { }},
        };

        private static void Quit()
        {
            Environment.Exit(0);
        }

        static void ChangeSeed()
        {
            uint seedValue;
            if (display.PromptForInt("Enter a positive integer for the new seed or press \'q\' to cancel.", out seedValue))
                Seed = seedValue;
        }

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
                var dungeon = Generator.Generate(size, Seed);
                display.ShowDungeon(dungeon);

                Thread.Sleep(100);
                display.ShowInstructions(Seed, size);

                var input = Console.ReadKey();
                Action result;
                if (_inputMap.TryGetValue(input.Key, out result))
                    result();

                Console.Clear();

                Seed++;
            }
        }

    }
}
