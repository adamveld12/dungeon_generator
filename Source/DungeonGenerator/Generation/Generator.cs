using Dungeon.Generator.Generation.Generators.GridBased;
using Dungeon.Generator.Navigation;

namespace Dungeon.Generator.Generation
{
    public class Generator
    {
        public static ITileMap Generate(MapSize size, uint seed)
        {
            var dim = size.ToDimensions();
            var map = new Dungeon(dim.X, dim.Y);
            var random = new MersennePrimeRandom(seed);

            var strategy = new GridBased(random, map);//new RoomGeneratorStrategy(random, map);
            strategy.Execute();

            return map;
            
        }

        public static ITileMap Generate(MapSize size)
        {
            return Generate(MapSize.Small, 1024u);
        }
    }

}