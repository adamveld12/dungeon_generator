namespace Dungeon.Generator
{
    public static class Generator
    {
        public static ITileMap Generate(MapSize size, uint seed)
        {
            var map = new TileMap((int) size, (int) size);

            new DungeonGenerator(map, seed).Generate();

            return map;
        }
    }
}