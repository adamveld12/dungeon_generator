using System.Diagnostics;

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

        public static Point operator *(Point a, int scalar) 
        { return new Point(a.X * scalar, a.Y * scalar); }

        public static Point operator +(Point a, int scalar)
        { return new Point(a.X + scalar, a.Y + scalar); }

        public static Point operator -(Point a, Point b)
        { return new Point(a.X - b.X, a.Y - b.Y); }

        public static Point operator +(Point a, Point b)
        { return new Point(a.X + b.X, a.Y + b.Y); }

        #region Properties

        public static Point Zero
        {
            get { return default(Point); }
        }

        #endregion
    }
}