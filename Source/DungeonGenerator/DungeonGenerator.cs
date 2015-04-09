using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dungeon.Generator
{
    public class DungeonGenerator
    {
        public const int CellSize = 9;
        private readonly ITileMap _map;
        private Cell[,] _cells;

        public DungeonGenerator(ITileMap map)
        {
            _map = map;
        }

        public void Generate()
        {
            var w = _map.Width/CellSize;
            var h = _map.Height/CellSize;

            _cells = new Cell[w,h];

            var startLoc = new Point { X = w/2, Y = h/2 };
            _cells[startLoc.X, startLoc.Y] = Cell.FourWayRoom();

            var unprocessed = new Queue<Point>();
            unprocessed.Enqueue(startLoc);

            while (unprocessed.Count > 0)
            {
                var location = unprocessed.Dequeue();

                var cell = _cells[location.X, location.Y];

                foreach(var opening in cell.Openings.ToDirectionsArray())
                {
                    var newLocation = opening.GetLocation(location);

                    var newCell = DetermineCellType(newLocation, opening);

                    if (newCell.Type != CellType.None)
                    {
                        _cells[newLocation.X, newLocation.Y] = newCell;
                        unprocessed.Enqueue(newLocation);
                    }
                }
            }

            for (var x = 0; x < _cells.GetLength(0); x++)
                for (var y = 0; y < _cells.GetLength(1); y++)
                    _cells[x, y].Fill(x, y, _map);

        }

        // pick a cell type that will connect as many rooms as possible
        private Cell DetermineCellType(Point location, Direction direction)
        {
            // 
            return default(Cell);
        }

    }

    public class CellGraphNode
    {
        private readonly LinkedList<CellGraphNode> _children = new LinkedList<CellGraphNode>();

        public CellGraphNode Add(Cell cell, int x, int y)
        {
            Debug.Assert(x == X && y == Y, "The cell passed has the same position as its parent");
            Debug.Assert(_children.Any(c => c.X == x && c.Y == y), "One of the children nodes is placed in the same location as the parent");

            var node = new CellGraphNode {
                Cell = cell,
                X = x,
                Y = y
            };

            _children.AddLast(node);

            return node;
        }

        public Cell Cell { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public IEnumerable<CellGraphNode> Children
        {
            get { return _children; }
        }
    }


    public struct Point
    {
        public int X { get; set; }  
        public int Y { get; set; }  
    }
}