using System;
using DungeonGenerator.Generation.Generators;
using DungeonGenerator.Navigation;

namespace DungeonGenerator.Generation
{
    public class Generator
    {
        // too much abstraction IMO
        private readonly IDungeonGenerationStrategy _strategy;

        public Generator(IDungeonGenerationStrategy strategy)
        {
            _strategy = strategy;
        }

        public ITileMap GenerateMap(MapSize size, uint seed)
        {
            var map = CreateMap(size);

            _strategy.Execute(map);

            return map;
        }

        private ITileMap CreateMap(MapSize size)
        {
            var dimensions = MapEditorTools.ToPoint(size);
            return new Dungeon(dimensions.X, dimensions.Y);
        }

        public static ITileMap Generate(MapSize size)
        {
            var gen = new Generator(new RoomFirstGeneratorStrategy());
            return gen.GenerateMap(MapSize.Small, 1024u);
        }
    }

}