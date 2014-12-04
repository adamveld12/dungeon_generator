using System.Collections.Generic;
using System.Linq;
using Dungeon.Generator.Navigation;

namespace Dungeon.Generator.Generation.Generators
{
    public class RoomGeneratorStrategy : IDungeonGenerationStrategy
    {
        private readonly MersennePrimeRandom _random;
        private readonly ITileMap _map;

        public RoomGeneratorStrategy(MersennePrimeRandom random, ITileMap map)
        {
            _random = random;
            _map = map;
            GridSize = 6;
        }

        public void Execute()
        {
            var mapCenter = new Point(_map.Width/2, _map.Height/2).ToGrid(GridSize);

            // spawn a room in the center
            var centerRoom = new Room {
                Location = mapCenter,
                Size = new Point(1, 1)
            };


            var unprocessed = new Queue<Room>();
            unprocessed.Enqueue(centerRoom);

            var directions = DirectionHelpers.Values();

            // for each unprocessed room
            while (unprocessed.Count > 0)
            {
                var room = unprocessed.Dequeue();

                // decide which directions to spawn rooms in
                var newRooms = directions
                    // if that direction is against the edge of the map, don't spawn there
                    .Where(x => room.Location.CanMove(x, _map))
                    // carve the outlets for the new rooms
                    .Where(x => {
                        // get the new room location
                        var newRoomLocation = room.Location.Move(x);

                        return _map.Contains(newRoomLocation.FromGrid(GridSize)) &&
                               !_map.ContainsRoom(GridSize, newRoomLocation);
                    })
                    // spawn rooms in those directions
                    .Select(direction => Room.CreateRoom(room.Location.Move(direction), direction));


                newRooms.Aggregate(unprocessed, (acc, spawnedRoom) => {
                    // add them to the unprocessed list
                    acc.Enqueue(spawnedRoom);
                    return acc;
                });

                // carve our room
                room.Carve(_map, GridSize);
            }
        }

        public int GridSize { get; set; }
    }

    public class Room
    {
        public RoomType Type { get; set; }
        public Point Size { get; set; }
        public Point Location { get; set; }
        public Direction Cardinality { get; set; }
 
        public static Room CreateCorridor(Direction direction, int length)
        {
            return new Room {
                Size = direction.Normal() * length,
                Type = RoomType.Corridor,
                Cardinality = direction
            };
        }

        public static Room CreateRoom(Point location, Direction direction)
        {
            return new Room {
                Type = RoomType.Room,
                Cardinality = direction,
                Location = location


            };
        }
    }

    public enum RoomType
    {
        Room = 0,
        Corridor,
        LeftTurn,
        RightTurn,
    }
}