using System;
using System.Linq;

namespace Dungeon.Generator
{
    /// <summary>
    /// Direction flags. Laid out so that a right turn is the same as going up one flag
    /// </summary>
    [Flags]
    public enum Direction : byte
    {
        None = 0,
        North = 0x01,
        East = 0x02,
        South = 0x04,
        West = 0x08
    }

    public static class DirectionHelpers
    {

        public static readonly Direction[] Directions = Enum.GetValues(typeof (Direction)).Cast<Direction>().Where(x => x != Direction.None).ToArray();

        public static bool HasFlag(this Direction dir, Direction other)
        {
            return (((byte) other) & ((byte) dir)) == (byte) dir;
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