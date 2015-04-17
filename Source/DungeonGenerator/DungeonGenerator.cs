using System.Collections.Generic;
using System.Linq;

namespace Dungeon.Generator
{
    internal class DungeonGenerator
    {
        private readonly GeneratorParams _params;
        public const int CellSize = 9;

        private MersennePrimeRandom _random;
        private Cell[,] _cells;

        public DungeonGenerator() : this(GeneratorParams.Default) { }
        public DungeonGenerator(GeneratorParams @params) { _params = @params; }

        public void Generate(ITileMap map)
        {
            _random = new MersennePrimeRandom(_params.Seed);

            // does two passes
            var w = map.Width/CellSize;
            var h = map.Height/CellSize;

            _cells = new Cell[w,h];


            // first pass populates an adjacency list with cell types in different opening combos
            var startLoc = new Point { X = w/2, Y = h/2 };
            _cells[startLoc.X, startLoc.Y] = Cell.FourWayRoom();

            if(_params.Exits)
                _cells[startLoc.X, startLoc.Y].Attributes = TileAttributes.Exit;

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

            var secondExitPlaced = !_params.Exits;

            // second pass bakes the adjacency list into the tile map
            for (var x = 0; x < _cells.GetLength(0); x++)
                for (var y = 0; y < _cells.GetLength(1); y++)
                {
                    if (!secondExitPlaced && (x <= w*0.15 || y > h*0.85))
                    {
                        _cells[x, y].Attributes = TileAttributes.Exit;
                        secondExitPlaced = true;
                    }

                    _cells[x, y].Fill(x, y, map, _params);
                }

            if (!secondExitPlaced)
            {
                _cells[w - 1, h - 1].Attributes = TileAttributes.Exit;
                _cells[w - 1, h - 1].Fill(w - 1, h - 1, map, _params);
            }
        }

        // pick a cell type that will connect as many rooms as possible
        private Cell DetermineCellType(Point location, Direction direction)
        {
            if (location.X >= 0 && location.X < _cells.GetLength(0) && location.Y >= 0 && location.Y < _cells.GetLength(0))
            {
                var cell = _cells[location.X, location.Y];
                if (cell.Type == CellType.None)
                {
                    var roomChance = _random.Next(0, 100)/100.0f;

                    return new Cell
                    {
                        Type = roomChance >= _params.RoomChance ? CellType.Room : CellType.Corridor,
                        Openings = FindValidConnections(direction, location)
                    };
                }
                
            }

            return default(Cell);
        }

        private Direction FindValidConnections(Direction dir, Point loc)
        {
            var list = new List<Direction>();

            var startDir = dir;

            dir = dir.TurnLeft();
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


            var connectsToMake = list.Count > 0 ? _random.Next(1, list.Count + 1) : 0;
            var validConnections =  list.Take(connectsToMake).Concat(new []{ startDir.TurnAround() }).ToArray();

            // close the openings in the neighbor cells that we didn't make
            foreach (var connectToUndo in list.Skip(connectsToMake))
            {
                var newLoc = connectToUndo.GetLocation(loc);
                var dirToUndo = connectToUndo.TurnAround();

                _cells[newLoc.X, newLoc.Y].Openings ^= dirToUndo;
            }

            return validConnections.ToDirectionFlag();
        }

    }
}