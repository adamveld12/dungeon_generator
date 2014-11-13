using System;
using System.Collections.Generic;
using System.Linq;
using DungeonGenerator.Builders;
using DungeonGenerator.Builders.Genetics;
using DungeonGenerator.Navigation;

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

            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                    _dungeon[x, y] =  TileType.Wall;

            var startY = _random.Next(1, height - 1);

            var builder = new Builder(1, startY, new Gene(1));
            builder.Location.Direction = Direction.E;

            // always start with one builder that's facing toward the center
            _dungeonBuilders = new List<Builder> {
                builder
            };
        }

        public bool Step()
        {
            if (_dungeonBuilders.Count > 0)
            {
                stepCount++;


                _dungeonBuilders = _dungeonBuilders // carve out some dungeon space
                                                   .Select(builder => {
                                                       builder.Step(Dungeon, stepCount, _random.Next());
                                                       return builder;
                                                   })
                                                   // reproduce builders
                                                   .SelectMany(builder => builder.Reproduce(_random.Next()))
                                                   // clear out dead builders
                                                   .Where(x => !x.IsDead)
                                                   .ToList();
            }

            return _dungeonBuilders.Count > 0;
        }

        public Dungeon Dungeon
        {
            get { return _dungeon; }
        }

        public int BuilderCount { get { return _dungeonBuilders.Count; } }
        public int Generation { get { return _dungeonBuilders.Count <= 0 ? -1 : _dungeonBuilders.Select(x => x.Genetics).Select(x => x.Generation).Max(); } }

    }
}