using System;
using System.Diagnostics;

namespace Dungeon.Generator.Navigation
{
    [DebuggerDisplay("Position:({X},{Y}) Size:({Width},{Height})")]
    public struct Room
    {
        public int X;
        public int Y;

        public int Width;
        public int Height;

        /// <summary>
        /// Sets the X/Y coordinate of the item to inside of this room
        /// </summary>
        /// <param name="item">The item to place</param>
        public Item PlaceItem(Item item)
        {
            var oldxy = item.Location;
            item.Location = new Point
            {
                X = X + (oldxy.X % Width),
                Y = Y + (oldxy.Y % Height)
            };
            return item;
        }

        public Point GetCenterWallPoint(Direction direction)
        {
            int x = X;
            int y = Y;
            switch (direction)
            {
                case Direction.N:
                    x += Width/2;
                    break;
                case Direction.E:
                    y += Height/2;
                    x += Width;
                    break;
                case Direction.S:
                    y += Height;
                    x += Width/2;
                    break;
                case Direction.W:
                    y += Height/2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }

            return new Point
            {
                Direction = direction, 
                X = x,
                Y = y,
            };
        }

        public bool Intersects(Room room)
        {
            var intersectingX = room.X + room.Width >= X && room.X <= X + Width;
            var intersectingY = room.Y + room.Height >= Y && room.Y <= Y + Height;

            return (intersectingX && intersectingY);
        }

        public Point Location
        {
            get { return new Point(X, Y); }
        }

        public Point Center
        {
            get {  return new Point(X + Width/2, Y + Height/2); }
        }
    }
}