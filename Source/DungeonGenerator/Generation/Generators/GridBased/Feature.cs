using System;
using System.Collections.Generic;
using Dungeon.Generator.Navigation;

namespace Dungeon.Generator.Generation.Generators.GridBased
{
    public struct Feature
    {
        public FeatureType Type;
        public Point Location;
        public Direction Direction;
    }

    public enum CorridorType
    {
        OneWayCorridor = 0,
        TwoWayCorridor,
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
            var direction = feature.Direction;

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
                    yield return direction;
                    break;
                default: throw new ArgumentOutOfRangeException("feature");
            }
        }


        public static void CarveCorridor(this Feature feature, CorridorType type, ITileMap map, int gridSize)
        {
            var location = feature.Location.FromGrid(gridSize);
            var direction = feature.Direction;
            switch (type)
            {
                case CorridorType.OneWayCorridor:
                    if(direction == Direction.N || direction == Direction.S)
                        map.Carve(location + new Point(gridSize/2, 1), 1, gridSize, 1);
                    else 
                        map.Carve(location + new Point(1, gridSize/2), gridSize, 1, 1);
                    break;
                case CorridorType.TwoWayCorridor:
                    break;
                case CorridorType.ThreeWayCorridor:
                    break;
                case CorridorType.FourWayCorridor:
                    break;
                case CorridorType.LeftTurnCorridor:
                    break;
                case CorridorType.RightTurnCorridor:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        public static void CarveRoom(this Feature feature, ITileMap map, int gridSize)
        {
            MovementHelpers.Carve(map, feature.Location.FromGrid(gridSize) + 1, gridSize - 1, gridSize - 1, 1);
        }

    }
}