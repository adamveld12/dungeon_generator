using Dungeon.Generator.Generation.Generators;
using Dungeon.Generator.Navigation;

namespace Dungeon.Generator.Generation
{
    public class Generator
    {
        public ITileMap GenerateMap(MapSize size, uint seed)
        {
            var map = CreateMap(size);
            var random = new MersennePrimeRandom(seed);

            var strategy = new RoomGeneratorStrategy(random, map);
            strategy.Execute();

            return map;
        }

        private ITileMap CreateMap(MapSize size)
        {
            var dimensions = size.ToDimensions();
            return new Dungeon(dimensions.X, dimensions.Y);
        }

        public static ITileMap Generate(MapSize size)
        {
            var gen = new Generator();
            return gen.GenerateMap(MapSize.Small, 1024u);
        }
    }

}