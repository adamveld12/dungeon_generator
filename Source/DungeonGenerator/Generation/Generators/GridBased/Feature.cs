using System;
using System.Collections.Generic;

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
    }
}