namespace Dungeon.Generator
{
    public static class Generator
    {
        public static ITileMap Generate(MapSize size, uint seed)
        {
            var map = new TileMap((int) size, (int) size);

            new DungeonGenerator().Generate(map, seed);

            return map;
        }

        public static ITileMap Generate(int width, int height, uint seed)
        {
            var map = new TileMap(width, height);

            new DungeonGenerator().Generate(map, seed);

            return map;
        }
    }
}