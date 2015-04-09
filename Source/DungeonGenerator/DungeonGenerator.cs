using System.Collections.Generic;

namespace Dungeon.Generator
{
    public class DungeonGenerator
    {
        private readonly ITileMap _map;
        private readonly LinkedList<Cell> _cells;
        private readonly Cell[,] _rooms;

        public DungeonGenerator(ITileMap map)
        {
            _map = map;
            _rooms = new Cell[map.Width/5, map.Height/5];
            _cells = new LinkedList<Cell>();
        }

        public void Generate()
        {
            var w = _map.Width;
            var h = _map.Height;

            // place room in middle of the map
            _rooms[w/2, h/2] = Cell.FourWayRoom();
        }

    }

}