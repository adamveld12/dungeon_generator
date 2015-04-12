namespace Dungeon.Generator
{
    internal class TileMap : ITileMap
    {
        private readonly TileType[,] _map;

        public TileMap(int width, int height)
        {
            Width = width;
            Height = height;

            _map = new TileType[Width,Height];
        }

        public TileType this[int x, int y]
        {
            get { return _map[x, y]; }
            set { _map[x, y] = value;  }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
    }
}