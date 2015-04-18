using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Dungeon.Generator
{
    internal class CellBasedGenerator
    {
        private readonly GeneratorParams _params;
        public const int CellSize = 9;

        private MersennePrimeRandom _random;
        private Cell[,] _cells;

        public CellBasedGenerator() : this(GeneratorParams.Default) { }
        public CellBasedGenerator(GeneratorParams @params) { _params = @params; }

        public void Generate(ITileMap map)
        {
            _random = new MersennePrimeRandom(_params.Seed);

            var w = map.Width/CellSize;
            var h = map.Height/CellSize;

            _cells = new Cell[w,h];

            var startLoc = new Point { X = w/2, Y = h/2 };
            _cells[startLoc.X, startLoc.Y] = Cell.FourWayRoom();

            if(_params.Exits)
                _cells[startLoc.X, startLoc.Y].Attributes = TileAttributes.Entry;

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
                        _cells[newLocation.X, newLocation.Y] = ApplyAttributes(newCell);
                        unprocessed.Enqueue(newLocation);
                    }
                }
            }

            var secondExitPlaced = !_params.Exits;

            var chance = 10;
            for (var x = 0; x < _cells.GetLength(0); x++)
                for (var y = 0; y < _cells.GetLength(1); y++)
                {
                    var cell = _cells[x, y];

                    if (!secondExitPlaced)
                    {
                        var spawnExit = _random.Chance(chance);
                        if (cell.Type != CellType.None && ((x <= w*0.15) || (x >= w*0.65)) && spawnExit)
                        {
                            cell.Attributes = TileAttributes.Exit;
                            secondExitPlaced = true;
                        }
                        else if (!spawnExit)
                            chance += (int)(chance * 0.25f);
                    }

                    cell.Fill(x, y, map, _params);
                }
        }


        private Cell ApplyAttributes(Cell newCell)
        {
            if (_random.Chance(_params.MonsterSpawns))
                newCell.Attributes |= TileAttributes.MonsterSpawn;

            if (_random.Chance(_params.Loot) && newCell.Type == CellType.Room)
                newCell.Attributes |= TileAttributes.Loot;

            if (_random.Chance(_params.Doors))
                newCell.Attributes |= TileAttributes.Doors;

            return newCell;
        }

        // pick a cell type that will connect as many rooms as possible
        private Cell DetermineCellType(Point location, Direction direction)
        {
            var locationInBounds = location.X >= 0 && location.X < _cells.GetLength(0) && location.Y >= 0 && location.Y < _cells.GetLength(0);
            if (locationInBounds && _cells[location.X, location.Y].Type == CellType.None)
                return new Cell
                {
                    Type = _random.Chance(_params.RoomChance) ? CellType.Room : CellType.Corridor,
                    Openings = FindValidConnections(direction, location),
                    Attributes = TileAttributes.None,
                };

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