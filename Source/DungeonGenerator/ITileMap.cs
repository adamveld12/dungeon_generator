﻿namespace Dungeon.Generator
{
    public class TileMap : ITileMap
    {
        private ushort[,] _map;

        public TileMap(int width, int height)
        {
            Width = width;
            Height = height;

            _map = new ushort[Width,Height];
        }

        public ushort this[int x, int y]
        {
            get { return _map[x, y]; }
            set { _map[x, y] = value;  }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
    }

}
