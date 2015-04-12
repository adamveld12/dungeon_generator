using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon.Generator
{
    /// <summary>
    /// Direction flags. Laid out so that a right turn is the same as going up one flag
    /// </summary>
    [Flags]
    internal enum Direction : byte
    {
        None = 0,
        North = 0x01,
        East = 0x02,
        South = 0x04,
        West = 0x08
    }

    internal static class DirectionHelpers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly IEnumerable<Direction> Directions = Enum.GetValues(typeof (Direction)).Cast<Direction>().Where(x => x != Direction.None).ToArray();

        public static bool HasFlag(this Direction dir, Direction other)
        {
            var result = (byte) dir & (byte) other;
            return result == (byte) other;
        }

        public static bool Facing(this Direction source, Direction other)
        {
            return source.ToDirectionsArray().Any(x =>
            {
                switch (x)
                {
                    case Direction.None: return false;
                    case Direction.North: return other.HasFlag(Direction.South);
                    case Direction.East: return other.HasFlag(Direction.West);
                    case Direction.South: return other.HasFlag(Direction.North);
                    case Direction.West: return other.HasFlag(Direction.East);
                    default: throw new ArgumentOutOfRangeException("x");
                }
            });
        }

        public static Direction ToDirectionFlag(this IEnumerable<Direction> source)
        {
            return source.Aggregate(Direction.None, (agg, item) => agg | item);
        }

        public static Point GetLocation(this Direction dir, Point old)
        {
            int x = 0, y = 0;

            switch (dir)
            {
                case Direction.North:
                    y = - 1;
                    break;
                case Direction.East:
                    x = 1;
                    break;
                case Direction.South:
                    y = 1;
                    break;
                case Direction.West:
                    x = -1;
                    break;
                case Direction.None: break;
                default:
                    throw new ArgumentOutOfRangeException("dir");
            }

            return new Point{X = old.X + x, Y = old.Y + y};
        }

        public static Direction[] ToDirectionsArray(this Direction dir)
        {
            return Directions.Where(x => dir.HasFlag(x)).ToArray();
        }

        public static Direction TurnAround(this Direction dir)
        {
            return dir.TurnRight().TurnRight();
        }

        public static Direction TurnLeft(this Direction dir)
        {
            return dir.TurnAround().TurnRight();
        }

        public static Direction TurnRight(this Direction dir)
        {
            switch (dir)
            {
                case Direction.North: return Direction.East;
                case Direction.East: return Direction.South;
                case Direction.South: return Direction.West;
                case Direction.West: return Direction.North;
                default: throw new ArgumentOutOfRangeException("dir");
            }
        }
    }
}