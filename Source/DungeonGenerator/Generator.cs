using System.Collections.Generic;
using System.Linq;

namespace DungeonGenerator
{
    public class Generator
    {
        
        public static Dungeon Generate()
        {
            var dungeon = new Dungeon(128, 24);

            for (var x = 0; x < dungeon.Width; x++)
            {
                for (var y = 0; y < dungeon.Height; y++)
                {
                    TileType tileType;

                    if (x == 0 || y == 0 || y == dungeon.Height - 1 || x == dungeon.Width - 1) 
                        tileType = TileType.Wall;
                    else
                        tileType = TileType.Floor;

                    dungeon[x, y] = tileType;
                }
            }
            return dungeon;
        }

        private readonly MersennePrimeRandom _random = new MersennePrimeRandom(34u);
        private readonly Dungeon _dungeon;
        private List<Builder> _dungeonBuilders;

        private int stepCount = 0;

        public Generator(int width, int height)
        {
            _dungeon = new Dungeon(width, height);
            _dungeonBuilders = Enumerable.Repeat(0, 1)
                                         .Select(x => new Builder(_random.Next(1, width - 2), _random.Next(1, height - 2)))
                                         .ToList();
        }

        public bool Step()
        {
            if (_dungeonBuilders.Count > 0)
            {
                stepCount++;
                _dungeonBuilders = _dungeonBuilders.Select(builder => {
                                                       builder.Step(_dungeon);
                                                       return builder;
                                                   })
                                                   .SelectMany(builder => {
                                                       if (builder.IsDead)
                                                           return builder.Reproduce(_random.Next());
                                                       return new[] {builder};
                                                   })
                                                   .Where(x => !x.IsDead)
                                                   .ToList();
            }

            return _dungeonBuilders.Count > 0;
        }
    }
}