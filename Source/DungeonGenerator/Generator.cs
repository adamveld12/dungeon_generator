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

            do { } while (gen.Step());

            return gen.Dungeon;
        }

        private readonly MersennePrimeRandom _random = new MersennePrimeRandom(34u);
        private readonly Dungeon _dungeon;

        private List<Builder> _builders;

        public Generator(int width, int height)
        {
            _dungeon = new Dungeon(width + 1, height + 1);
            _builders = new List<Builder>();

            const int gridSize = 8;

            // generate rooms along a grid of 4 x 4 tiles
            for (int x = 0; x < width/gridSize; x++)
                for (int y = 0; y < height/gridSize; y++)
                {
                    // add one to garauntee that we don't have a room off of the edge
                    var realX = x*gridSize + 1;
                    var realY = y*gridSize + 1;

                    var chance = _random.Next(100);

                    // 60% chance of there being a room in a cell
                    if (chance < 60)
                    {
                        var roomWidth = gridSize - 1;
                        var roomHeight = gridSize - 1;

                        var builderLocationX = realX;
                        var builderLocationY = realY;

                        _builders.Add(new RoomBuilder {
                            Location = new Location { X = builderLocationX, Y = builderLocationY },
                            Height = roomWidth,
                            Width = roomHeight
                        });
                        
                    }
                }

        }

        public bool Step()
        {
            var buildersQuery = _builders.AsParallel();


            _builders = buildersQuery.SelectMany(x => {
                x.Build(_dungeon);
                return x.Spawn();
            }).ToList();

            return _builders.Count > 0;
        }

        public Dungeon Dungeon
        {
            get { return _dungeon; }
        }
    }

}