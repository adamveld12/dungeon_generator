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
        LeftTurnCorridor,
        RightTurnCorridor
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

            switch (CorridorType)
            {
                // corridor without turns
                case CorridorType.OneWayCorridor:
                    var dirNormal = direction.TurnLeft().Normal();
                    dirNormal.X = Math.Abs(dirNormal.X);
                    dirNormal.Y = Math.Abs(dirNormal.Y);
                    var locationDelta = new Point(dirNormal.X * offset, dirNormal.Y * offset);

                    var carveNormal = direction.Normal();
                    carveNormal.X = Math.Abs(carveNormal.X);
                    carveNormal.Y = Math.Abs(carveNormal.Y);

                    var carveDelta = carveNormal*gridSize;
                    carveDelta.X = Math.Max(carveDelta.X, 1);
                    carveDelta.Y = Math.Max(carveDelta.Y, 1);

                    map.Carve(location + locationDelta, carveDelta.X, carveDelta.Y, 1);
//                    if(direction == Direction.N || direction == Direction.S)
//                        map.Carve(location + new Point(gridSize/2, 0), 1, gridSize, 1);
//                    else 
//                        map.Carve(location + new Point(0, gridSize/2), gridSize, 1, 1);
                    break;
                // corridor that turns left and right
                case CorridorType.ThreeWayCorridor:
                    if (direction == Direction.N || direction == Direction.S) {
                        // entry
                        map.Carve(location + new Point(gridSize/2, 0), 1, gridSize/2, 1);
                        // fork
                        map.Carve(location + new Point(0, gridSize/2), gridSize, 1, 1);
                    }
                    else {
                        // entry
                        map.Carve(location + new Point(0, gridSize/2), gridSize/2, 1, 1);
                        // fork
                        map.Carve(location + new Point(gridSize/2, 0), 1, gridSize, 1);
                    }

                    break;
                // a cross
                case CorridorType.FourWayCorridor:
                    // entry
                    map.Carve(location + new Point(gridSize/2, 1), 1, gridSize, 1);
                    // fork
                    map.Carve(location + new Point(1, gridSize/2), gridSize, 1, 1);
                    break;
                case CorridorType.LeftTurnCorridor:
                    break;
                case CorridorType.RightTurnCorridor:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        private void CarveRoom(ITileMap map, int gridSize)
        {
            map.Carve(Location.FromGrid(gridSize) + 1, gridSize - 1, gridSize - 1, 1);
        }

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
                    break;
                case CorridorType.LeftTurnCorridor:
                    yield return direction.TurnLeft();
                    break;
                case CorridorType.RightTurnCorridor:
                    yield return direction.TurnRight();
                    break;
                case CorridorType.ThreeWayCorridor:
                    yield return direction.TurnLeft();
                    yield return direction.TurnRight();
                    break;
                case CorridorType.FourWayCorridor:
                    yield return direction.TurnLeft();
                    yield return direction.TurnRight();
                    yield return direction;
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