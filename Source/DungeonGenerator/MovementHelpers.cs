using System;
using System.Collections.Generic;
using Dungeon.Generator.Navigation;

namespace Dungeon.Generator
{
    public static class MovementHelpers
    {
        private static readonly Dictionary<Direction, Point> MovementDeltas = new Dictionary<Direction, Point>
        {
            { Direction.N, new Point(0, -1) },
            { Direction.S, new Point(0, 1)  },
            { Direction.E, new Point(1, 0)  },
            { Direction.W, new Point(-1, 0) },
        };

        /// <summary>
        /// Gets the normal point for the given direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Point Normal(this Direction direction)
        {
            return MovementDeltas[direction];
        }

        /// <summary>
        /// Moves the given point 1 coordinate in the given direction
        /// </summary>
        /// <param name="point"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Point Move(this Point point, Direction direction)
        {
            if (!MovementDeltas.ContainsKey(direction)) 
                throw new ArgumentException("direction");

            return MovementDeltas[direction] + point;
        }

        /// <summary>
        /// Transforms the given point from grid space to raw tile coordinate space
        /// </summary>
        /// <param name="point"></param>
        /// <param name="gridSize"></param>
        /// <returns></returns>
        public static Point FromGrid(this Point point, int gridSize)
        { return new Point(point.X * gridSize, point.Y * gridSize); }

        /// <summary>
        /// Transforms the given point into grid space
        /// </summary>
        /// <param name="point"></param>
        /// <param name="gridSize"></param>
        /// <returns></returns>
        public static Point ToGrid(this Point point, int gridSize)
        { return new Point(point.X / gridSize, point.Y / gridSize); }

        /// <summary>
        /// gives the 'center' of the given tile map in raw coordinates
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static Point Center(ITileMap map)
        { return new Point(map.Width/2, map.Height/2); }


        /// <summary>
        /// If the given point can move 1 step in the direction specified
        /// </summary>
        /// <param name="point"></param>
        /// <param name="direction"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static bool CanMove(this Point point, Direction direction, ITileMap map)
        {
            var newLocation = point.Move(direction);

            return map.Contains(newLocation);
        }

        /// <summary>
        /// If the given point is within the <see cref="ITileMap"/> bounds
        /// </summary>
        /// <param name="map"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static bool Contains(this ITileMap map, Point location)
        {

            return location.X >= 0 && location.X < map.Width
                   && location.Y >= 0 && location.Y < map.Height;
        }

        /// <summary>
        /// Rotates the given direction to the left and returns the result
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Direction TurnLeft(this Direction direction)
        {
            var newDirection = (int)(direction - 1);
            newDirection = newDirection%((int) Direction.Min);
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
            newDirection = newDirection%((int) Direction.Max);
            return (Direction) newDirection;
        }
    }
}