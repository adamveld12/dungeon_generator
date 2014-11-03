using System.Collections.Generic;
using System.Linq;

namespace DungeonGenerator
{
    public class Generator
    {
        
        public static Dungeon Generate()
        {
            var gen = new Generator(128, 24);

            do { } while (gen.Step());

            return gen.Dungeon;
        }

        private readonly MersennePrimeRandom _random = new MersennePrimeRandom(34u);
        private readonly Dungeon _dungeon;
        private List<Builder> _dungeonBuilders;

        private int stepCount = 0;

        public Generator(int width, int height)
        {
            _dungeon = new Dungeon(width, height);
            _dungeonBuilders = Enumerable.Repeat(0, 7)
                                         .Select(x => new Builder(_random.Next(1, width - 2), _random.Next(1, height - 2)))
                                         .ToList();

            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                    _dungeon[x, y] =  TileType.Wall;;
        }

        private bool Step()
        {
            if (_dungeonBuilders.Count > 0)
            {
                stepCount++;
                _dungeonBuilders = _dungeonBuilders.AsParallel()
                                                   .Select(builder => {
                                                       builder.Step(Dungeon, stepCount, _random.Next());
                                                       return builder;
                                                   })
                                                   .SelectMany(builder => {
                                                       if (builder.IsDead)
                                                           return builder.Reproduce(_random.Next());
                                                       return new[] {builder};
                                                   }).Where(x => !x.IsDead)
                                                   .ToList();
            }

            return _dungeonBuilders.Count > 0;
        }

        public Dungeon Dungeon
        {
            get { return _dungeon; }
        }
    }
}