namespace DungeonGenerator
{
    public class Dungeon : ITileMap
    {
        private readonly int _width;
        private readonly int _height;
        private readonly TileType[,] _map;

        public Dungeon(int width, int height)
        {
            _width = width;
            _height = height;
            _map = new TileType[width,height];
        }

        public TileType this[int x, int y]
        {
            get { return _map[x, y]; }
            set { _map[x, y] = value; }
        }

        public int Height
        {
            get { return _height; }
        }

        public int Width
        {
            get { return _width; }
        }
    }
}