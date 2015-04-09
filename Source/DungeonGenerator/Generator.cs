namespace Dungeon.Generator
{
    public static class Generator
    {
        public static ITileMap Generate(MapSize size)
        {
            var map = new TileMap((int) size, (int) size);

            new DungeonGenerator(map).Generate();

            return map;
        }


    }
}