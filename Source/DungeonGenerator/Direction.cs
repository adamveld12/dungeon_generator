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
    }

}