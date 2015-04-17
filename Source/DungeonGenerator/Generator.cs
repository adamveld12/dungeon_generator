namespace Dungeon.Generator
{
    public static class Generator
    {
        public static ITileMap Generate(MapSize size, GeneratorParams parameters)
        {
            var map = new TileMap((int) size, (int) size);
            new DungeonGenerator(parameters).Generate(map);
            return map;
        }

        public static ITileMap Generate(MapSize size, uint seed)
        {
            var map = new TileMap((int) size, (int) size);

            var parameters = GeneratorParams.Default;
            parameters.Seed = seed;

            new DungeonGenerator(parameters).Generate(map);

            return map;
        }

        public static ITileMap Generate(int width, int height, uint seed)
        {
            var map = new TileMap(width, height);

            var parameters = GeneratorParams.Default;
            parameters.Seed = seed;

            new DungeonGenerator(parameters).Generate(map);

            return map;
        }
    }
}