namespace Dungeon.Generator
{
    public interface ITileMap
    {
        TileType this[int x, int y] { get; set; }
        int Width { get; }
        int Height { get; }
    }
}
