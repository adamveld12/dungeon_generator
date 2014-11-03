namespace DungeonGenerator
{
    public interface ITileMap
    {
        TileType this[int x, int y] { get; set; }
        int Height { get; }
        int Width { get; }
    }
}