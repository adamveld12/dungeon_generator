using System.Collections.Generic;
using System.Linq;
using DungeonGenerator.Navigation;

namespace DungeonGenerator
{
    public class Generator
    {
        public static Dungeon Generate()
        {
            var gen = new Generator(128, 24);

            do
            {
                
            } while (gen.Step());

            return gen.Dungeon;
        }

        private readonly MersennePrimeRandom _random = new MersennePrimeRandom(34u);
        private readonly Dungeon _dungeon;

        public Generator(int width, int height)
        {
            
        }

        public bool Step()
        {
            
        }

        public Dungeon Dungeon
        {
            get { return _dungeon; }
        }
    }
}