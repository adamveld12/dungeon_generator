namespace Dungeon.Generator
{
    public interface ITileMap
    {
        Tile this[int x, int y] { get; set; }
        int Width { get; }
        int Height { get; }
    }
}
