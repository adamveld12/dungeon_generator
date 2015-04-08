namespace Dungeon.Generator
{
    public interface ITileMap
    {
        ushort this[int x, int y] { get; set; }
        int Width { get; }
        int Height { get; }
    }
}