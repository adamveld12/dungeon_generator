using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dungeon.Generator;

namespace Demo
{
    public class Program
    {
        private static readonly MapSize[] sizes = Enum.GetValues(typeof (MapSize)).Cast<MapSize>().ToArray();
        private static int selectedSize;
        private static uint Seed = 1032u;
        private static ITileMap dungeon;
        private static bool running;

        private static readonly Display display = new Display();
        private static readonly Dictionary<ConsoleKey, Action>  _inputMap = new Dictionary<ConsoleKey, Action>
        {
            {ConsoleKey.W, IncreaseSize},
            {ConsoleKey.S, DecreaseSize},
            {ConsoleKey.C, ChangeSeed},
            {ConsoleKey.Q, Quit},
            {ConsoleKey.Enter, Generate},
        };

        public static void Main()
        {
            running = true;


            while (running)
            {
                Generate();
                display.ShowDungeon(dungeon);

                Thread.Sleep(100);

                display.ShowInstructions(Seed, sizes[selectedSize]);

                Action result;
                ConsoleKeyInfo input;
                do
                {
                    input = Console.ReadKey();
                } while (!_inputMap.TryGetValue(input.Key, out result));

                result();

                Console.Clear();

                Seed++;
            }
        }

        private static void Generate()
        {
            var size = sizes[selectedSize];
            dungeon = Generator.Generate(size, Seed);
        }

        private static void Quit()
        {
            running = false;
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
    }
}
