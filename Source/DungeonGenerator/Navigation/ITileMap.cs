namespace DungeonGenerator.Navigation
{
    public interface ITileMap
    {
        ushort this[int x, int y] { get; set; }

        int Height { get; }
        int Width { get; }
    }

    public struct Room
    {
        public int X;
        public int Y;

        public int Width;
        public int Height;
    }


    public struct Item
    {
        public Point Location;
        public byte Type;
    }
}