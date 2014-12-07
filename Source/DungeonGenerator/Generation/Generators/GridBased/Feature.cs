using System;
using System.Collections.Generic;
using System.Linq;
using Dungeon.Generator.Navigation;

namespace Dungeon.Generator.Generation.Generators.GridBased
{
    public enum CorridorType
    {
        OneWayCorridor = 0,
        ThreeWayCorridor,
        FourWayCorridor,
//        LeftTurnCorridor,
//        RightTurnCorridor
    }

    public enum FeatureType : byte
    {
        None = 0,
        Room = 0x01,
        Corridor = 0x10,
    }

    public class Feature
    {
        public Point Location;
        public Feature Origin;
        public FeatureType Type;
        public CorridorType CorridorType;
        public IEnumerable<Feature> Connections = Enumerable.Empty<Feature>();

        public  Direction GetDirectionOf( Feature other)
        {
            var location = other.Location;

            if (location.X > Location.X)
                return Direction.E;
            if (location.X < Location.X)
                return Direction.W;
            if (location.Y > Location.Y)
                return Direction.S;
            if (location.Y < Location.Y)
                return Direction.N;
            
            throw new InvalidOperationException();
        }

        public void Carve(ITileMap map, int gridSize)
        {
            // carve the inlet
            if (Origin != null)
            {
                var wall = Location.GetCenterWallPoint(GetDirectionOfOrigin(), gridSize);
                map.Carve(wall, 1, 1, 1);
            }

            // carve the outlets
            Connections.Where(feature => {
                var direction = GetDirectionOf(feature);
                var wallLocation = Location.GetCenterWallPoint(direction, gridSize);
                map.Carve(wallLocation, 1, 1, 1);
                return true;
            }).ToArray();

            if(Type == FeatureType.Room) CarveRoom(map, gridSize);
            else if (Type == FeatureType.Corridor) CarveCorridor(map, gridSize);
        }

        public Direction GetDirectionOfOrigin() { return GetDirectionOf(Origin); }

        private void CarveCorridor(ITileMap map, int gridSize)
        {
            var location = Location.FromGrid(gridSize);
            var direction = GetDirectionOfOrigin();
            var offset = gridSize/2;

            // corridor without turns
            if (CorridorType == CorridorType.OneWayCorridor)
            {
                var carveNormal = direction.Normal().Abs();
                var carveDelta = carveNormal*gridSize;
                carveDelta.X = Math.Max(carveDelta.X, 1);
                carveDelta.Y = Math.Max(carveDelta.Y, 1);

                var dirNormal = direction.TurnLeft().Normal().Abs();
                var locationDelta = dirNormal * offset;
                map.Carve(location + locationDelta, carveDelta.X, carveDelta.Y, 1);
                
            }
                // corridor that turns left and right
            else if (CorridorType == CorridorType.ThreeWayCorridor)
            {
                var entryNormal = direction.Normal().Abs();
                var forkNormal = direction.TurnLeft().Normal().Abs();

                if (direction == Direction.N)
                {
                    //entry from S
                    map.Carve(location + new Point(gridSize/2, 0), 1, gridSize/2, 1);
                    // fork E/W
                    map.Carve(location + new Point(0, gridSize/2), gridSize, 1, 1);
                }
                else if (direction == Direction.S) {

//                    // entry
//                    var entryOffset = entryNormal*gridSize/2;
//                    var entryCarve = entryNormal*gridSize/2;
//                    entryCarve.X = Math.Max(entryCarve.X, 1);
//                    entryCarve.Y = Math.Max(entryCarve.Y, 1);
//
//                    map.Carve(location + entryOffset, 1, gridSize/2, 1);
//
//                    // fork
//                    var forkOffset = forkNormal*gridSize/2;
//                    var forkCarve = entryNormal*gridSize/2;
//                    forkCarve.X = Math.Max(forkCarve.X, 1);
//                    forkCarve.Y = Math.Max(forkCarve.Y, 1);
//
//                    map.Carve(location + forkOffset, forkCarve.X, forkCarve.Y, 1);

                    //entry from N
                    map.Carve(location + new Point(gridSize/2, gridSize/2), 1, gridSize/2, 1);
                    // fork E/W
                    map.Carve(location + new Point(0, gridSize/2), gridSize, 1, 1);
                }
                else if (direction == Direction.E)
                {
                    // entry from W
                    map.Carve(location + new Point(gridSize/2, gridSize/2), gridSize/2, 1, 1);
                    // fork
                    map.Carve(location + new Point(gridSize/2, 0), 1, gridSize, 1);
                }
                else if (direction == Direction.W)
                {
                    // entry from W
                    map.Carve(location + new Point(0, gridSize/2), gridSize/2, 1, 1);
                    // fork
                    map.Carve(location + new Point(gridSize/2, 0), 1, gridSize, 1);
                }
            }
            else if (CorridorType == CorridorType.FourWayCorridor)
            {
                // entry
                map.Carve(location + new Point(gridSize/2, 1), 1, gridSize, 1);
                // fork
                map.Carve(location + new Point(1, gridSize/2), gridSize, 1, 1);
            }
            else
                throw new InvalidOperationException();
        }

        private void CarveRoom(ITileMap map, int gridSize)
        { map.Carve(Location.FromGrid(gridSize) + 1, gridSize - 1, gridSize - 1, 1); }

        public IEnumerable<Direction> ConnectionCardinality
        {
            get { return Connections.Select(GetDirectionOf); }
        }
    }

    public static class FeatureHelpers
    {
        public static IEnumerable<Direction> Walls(this CorridorType corridorType, Direction direction)
        {
            switch (corridorType)
            {
                case CorridorType.OneWayCorridor:
                    yield return direction;
                    yield return direction.TurnLeft().TurnLeft();
                    break;
//                case CorridorType.LeftTurnCorridor:
//                    yield return direction.TurnLeft();
//                    break;
//                case CorridorType.RightTurnCorridor:
//                    yield return direction.TurnRight();
//                    break;
                case CorridorType.ThreeWayCorridor:
                    yield return direction.TurnLeft();
                    yield return direction.TurnRight();
                    break;
                case CorridorType.FourWayCorridor:
                    yield return direction;
                    yield return direction.TurnLeft();
                    yield return direction.TurnRight();
                    yield return direction.TurnRight().TurnRight();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("corridorType");
            }
        }

        public static IEnumerable<Direction> Walls(this Feature feature)
        {
            var featureType = feature.Type;

            switch (featureType)
            {
                case FeatureType.None:
                    break;
                case FeatureType.Room:
                    yield return Direction.N;
                    yield return Direction.W;
                    yield return Direction.S;
                    yield return Direction.E;
                    break;
                case FeatureType.Corridor:
                    var direction = feature.GetDirectionOfOrigin();
                    foreach (var wall in feature.CorridorType.Walls(direction))
                        yield return wall;
                    break;
                default: throw new ArgumentOutOfRangeException("feature");
            }
        }
    }
}