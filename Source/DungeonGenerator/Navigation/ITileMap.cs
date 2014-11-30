namespace Dungeon.Generator.Navigation
{
    public interface ITileMap
    {
        ushort this[int x, int y] { get; set; }

        int Height { get; }
        int Width { get; }
    }
}