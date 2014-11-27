using System;
using System.Linq;

namespace Dungeon.Generator
{
    public enum Direction : byte
    {
        N = 0, 
        E, 
        S, 
        W,
        Max = W,
        Min = N
    }


    public static class DirectionHelpers
    {
        public static readonly Direction[] _values = Enum.GetValues(typeof (Direction)).Cast<Direction>().Distinct().ToArray();
        public static Direction[] Values()
        {
            return _values;
        }

        /// <summary>
        /// Rotates the given direction to the left and returns the result
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Direction TurnLeft(this Direction direction)
        {
            var newDirection = (int)(direction - 1);
            newDirection = Math.Min(newDirection, ((int) Direction.Max));
            return (Direction) newDirection;
        }

        /// <summary>
        /// Rotates the given direction to the right and returns the result
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Direction TurnRight(this Direction direction)
        {
            var newDirection = (int)(direction + 1);
            newDirection = newDirection % ((int) Direction.Max);
            return (Direction) newDirection;
        }
    }

}