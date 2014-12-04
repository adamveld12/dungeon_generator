using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dungeon.Generator.Navigation;

namespace Dungeon.Generator
{
    [DebuggerDisplay("({X},{Y})")]
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        #region Properties

        public static Point Zero
        {
            get { return default(Point); }
        }

        #endregion
    }

    public static class MovementHelpers
    {
        private static readonly Dictionary<Direction, Point> _movementDeltas = new Dictionary<Direction, Point>
        {
            {Direction.N, new Point(0, -1)},
            {Direction.S, new Point(0, 1)},
            {Direction.E, new Point(1, 0)},
            {Direction.W, new Point(-1, 0)},
        };

        public static Point Move(this Point point, Direction direction)
        {
            if (!_movementDeltas.ContainsKey(direction)) 
                throw new ArgumentException("direction");

            return _movementDeltas[direction] + point;
        }

    }
}