using System;
using System.Collections.Generic;

namespace Dungeon.Generator.Navigation
{
    public struct Point
    {
        #region Fields

        private int _x;
        private int _y;
        private int _totalStepsTaken;
        private Direction _direction;

        private static readonly IDictionary<Direction, Tuple<int, int>> _directionMovementDeltas = new Dictionary<Direction, Tuple<int, int>>{
            {Direction.N, new Tuple<int, int>(0, -1)},
            {Direction.S, new Tuple<int, int>(0, 1)},
            {Direction.E, new Tuple<int, int>(1, 0)},
            {Direction.W, new Tuple<int, int>(-1, 0)},
        };

        #endregion

        public Point(int x, int y)
        {
            _x = x;
            _y = y;
            _totalStepsTaken = 0;
            _direction = Direction.N;
        }

        public bool CanWalk(ITileMap tilemap, int builderWidth)
        {
            var width = tilemap.Width - 1 - builderWidth;
            var height = tilemap.Height - 1 - builderWidth;

            var tuple = _directionMovementDeltas[_direction];

            var deltaX = tuple.Item1;
            var deltaY = tuple.Item2;

            return (X + deltaX >= 1 && X + deltaX <= width) && (Y + deltaY >= 1 && Y + deltaY <= height);
        }

        public void Walk()
        {
            var tuple = _directionMovementDeltas[_direction];

            var deltaX = tuple.Item1;
            var deltaY = tuple.Item2;

            X += deltaX;
            Y += deltaY;
            _totalStepsTaken++;
        }

        public void TurnLeft()
        {
            var directionVal = (int)_direction - 1;
            _direction = directionVal < 0 ? Direction.Max : (Direction)directionVal;
        }

        public void TurnRight()
        {
            var directionVal = (int)_direction + 1;
            _direction = (Direction)(directionVal % (int)Direction.Max);
        }

        public ushort LookAhead(ITileMap map)
        {
            if (CanWalk(map, 1))
            {
                var tuple = _directionMovementDeltas[_direction];

                var deltaX = X + tuple.Item1;
                var deltaY = Y + tuple.Item2;
                return map[deltaX, deltaY];
            }

            return 0;
        }

        #region Properties

        public int TotalStepsTaken
        {
            get { return _totalStepsTaken; }
        }

        public Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        #endregion

    }
}