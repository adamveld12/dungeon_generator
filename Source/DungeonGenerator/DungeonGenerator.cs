using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon.Generator
{
    internal class DungeonGenerator
    {
        public const int CellSize = 9;

        private MersennePrimeRandom _random;
        private Cell[,] _cells;

        public void Generate(ITileMap map, uint seed)
        {
            _random = new MersennePrimeRandom(seed);

            var w = map.Width/CellSize;
            var h = map.Height/CellSize;

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
                    _cells[x, y].Fill(x, y, map);
        }

        // pick a cell type that will connect as many rooms as possible
        private Cell DetermineCellType(Point location, Direction direction)
        {
            if (location.X >= _cells.GetLength(0) || location.Y >= _cells.GetLength(1) || location.X < 0 || location.Y < 0)
                return default(Cell);

            var cell = _cells[location.X, location.Y];
            if (cell.Type == CellType.None)
            {
                // check the three directions
                var connections = FindValidConnections(direction, location);

                // pick any random cell type to connect them
                var types = CellHelpers.CellTypes;
                var cellType = types.ElementAt(_random.Next(types.Count()));

                var connectsToMake = connections.Length > 0 ? _random.Next(1, connections.Length + 1) : 0; //_random.Next(1, connections.Length + 1);

                // pick any number of them at random to connect
                connections = connections.Take(connectsToMake).Concat(new []{ direction.TurnAround() }).ToArray();

                return new Cell
                {
                    Type = cellType,
                    Openings = connections.ToDirectionFlag()
                };
            }

            return default(Cell);
        }

        private Direction[] FindValidConnections(Direction dir, Point loc)
        {
            var list = new List<Direction>();

            for (var i = 0; i < DirectionHelpers.Directions.Count() - 1; i++)
            {
                var newLoc = dir.GetLocation(loc);

                if (newLoc.X >= 0 && newLoc.X < _cells.GetLength(0) && newLoc.Y >= 0 && newLoc.Y < _cells.GetLength(1))
                {
                    var cell = _cells[newLoc.X, newLoc.Y];

                    if(cell.Type == CellType.None || cell.Openings.Facing(dir))
                        list.Add(dir);
                }

                dir = dir.TurnRight();
            }

            return list.ToArray();
        }

    }
}