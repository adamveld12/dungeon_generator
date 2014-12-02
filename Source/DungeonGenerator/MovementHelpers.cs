using System;
using System.Collections.Generic;
using Dungeon.Generator.Generation.Generators.RoomBased;
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


        public static Point GetCenterWallPoint(this Room room, Direction direction, int gridSize)
        {
            var delta = MovementDeltas[direction];
            var center = room.Location.FromGrid(gridSize) + (gridSize/2);

            switch (room.Type)
            {
                case RoomType.Room:
                    return (delta*(gridSize/2)) + center;
                case RoomType.Corridor:
                    return Point.Zero;
                case RoomType.LeftTurn:
                    return Point.Zero;
                case RoomType.RightTurn:
                    return Point.Zero;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Point GetCenterWallPoint(this Point location, Direction direction, int gridSize)
        {
            var delta = MovementDeltas[direction];
            var center = location.FromGrid(gridSize) + (gridSize/2);
            return (delta*(gridSize/2)) + center;
        }
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
            return location.X >= 0 && location.X < map.Width && location.Y >= 0 && location.Y < map.Height;
        }

        /// <summary>
        /// If the given point is within the <see cref="ITileMap"/> bounds
        /// </summary>
        /// <param name="map"></param>
        /// <param name="location"></param>
        /// <param name="gridSize"></param>
        /// <returns></returns>
        public static bool Contains(this ITileMap map, Point location, int gridSize)
        {
            return location.X >= 0 && location.X < map.Width/gridSize && location.Y >= 0 && location.Y < map.Height/gridSize;
        }

        public static bool ContainsRoom(this ITileMap map, int gridSize, Point location)
        {
            for(int x = location.X; x < location.X + gridSize; x++)
                for(int y = location.Y; y < location.Y + gridSize; y++)
                    if (map[x, y] != 0)
                        return true;
            return false;
        }

        /// <summary>
        /// Rotates the given direction to the left and returns the result
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Direction TurnLeft(this Direction direction)
        {
            var newDirection = (int)(direction - 1);
            newDirection = Math.Min(newDirection, ((int) Direction.N));
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

        /// <summary>
        /// Carves a room out of the <see cref="ITileMap"/>
        /// </summary>
        /// <param name="room"></param>
        /// <param name="map"></param>
        /// <param name="gridSize"></param>
        public static void Carve(this Room room, ITileMap map, int gridSize)
        {
            switch (room.Type)
            {
                case RoomType.Room:
                    Carve(map, room.Location.FromGrid(gridSize) + 1, gridSize - 1, gridSize - 1, 1);
                    break;
                case RoomType.Corridor:
                    var delta = MovementDeltas[room.Cardinality] * -1;
                    switch (room.Cardinality)
                    {
                        case Direction.N:
                        case Direction.S:
                            Carve(map, room.Location.FromGrid(gridSize) + new Point(gridSize/2, 0),  1,  gridSize,  1);
                            break;
                        case Direction.E:
                        case Direction.W:
                            Carve(map, room.Location.FromGrid(gridSize) + new Point(0, gridSize/2),  gridSize,  1,  1);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    var carveOriginOffset = (delta * (gridSize/2));
                    break;
                case RoomType.LeftTurn:
                    break;
                case RoomType.RightTurn:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// If the given point can move 1 step in the direction specified
        /// </summary>
        /// <param name="point"></param>
        /// <param name="direction"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static bool CanMove<T>(this Point point, Direction direction, T[,] map)
        {
            var newLocation = point.Move(direction);

            return map.Contains(newLocation);
        }

        public static bool Contains<T>(this T[,] map, Point location)
        {
            return location.X >= 0 && location.X < map.GetLength(0) && location.Y >= 0 && location.Y < map.GetLength(1);
        }

        /// <summary>
        /// Carves a box into the map
        /// </summary>
        /// <param name="map"></param>
        /// <param name="location"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="type"></param>
        public static void Carve(this ITileMap map, Point location, int width, int height, ushort type)
        {
            if (width < 0 || height < 0 || !map.Contains(location))
                throw new ArgumentOutOfRangeException();

            for (int x = location.X; x < location.X + width; x++)
                for (int y = location.Y; y < location.Y + height; y++)
                    map[x, y] = type;
        }
    }
}