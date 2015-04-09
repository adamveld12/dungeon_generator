using System.Collections.Generic;

namespace Dungeon.Generator
{
    public class DungeonGenerator
    {
        public const int CellSize = 9;
        private readonly ITileMap _map;
        private readonly MersennePrimeRandom _random;
        private Cell[,] _cells;

        public DungeonGenerator(ITileMap map, uint seed)
        {
            _map = map;
            _random = new MersennePrimeRandom(seed);
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
            var newLocation = direction.GetLocation(location);

            if (newLocation.X >= _cells.GetLength(0) || newLocation.Y >= _cells.GetLength(1))
                return default(Cell);

            var cell = _cells[newLocation.X, newLocation.Y];
            if (cell.Type == CellType.None)
            {
                var connections = new List<Direction>();

                // check the three directions
                // pick any number of them at random to connect


            }

            return default(Cell);
        }

    }
}