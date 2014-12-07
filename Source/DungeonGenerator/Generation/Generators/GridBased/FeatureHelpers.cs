using System;
using System.Collections.Generic;

namespace Dungeon.Generator.Generation.Generators.GridBased
{
    internal static class FeatureHelpers
    {
        public static IEnumerable<Direction> Walls(this CorridorType corridorType, Direction direction)
        {
            switch (corridorType)
            {
                case CorridorType.OneWayCorridor:
                    yield return direction;
                    yield return direction.TurnLeft().TurnLeft();
                    break;
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