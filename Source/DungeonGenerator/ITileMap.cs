namespace Dungeon.Generator
{
    public interface ITileMap
    {
        TileAttributes this[int x, int y] { get; set; }
        int Width { get; }
        int Height { get; }
    }
}
