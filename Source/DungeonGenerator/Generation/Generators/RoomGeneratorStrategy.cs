using System.Collections.Generic;
using System.Linq;
using Dungeon.Generator.Generation.Generators.RoomBased;
using Dungeon.Generator.Navigation;

namespace Dungeon.Generator.Generation.Generators
{
    public class RoomGeneratorStrategy : IDungeonGenerationStrategy
    {
        private readonly MersennePrimeRandom _random;
        private readonly ITileMap _map;

        private Room[,] _roomGrid;

        public RoomGeneratorStrategy(MersennePrimeRandom random, ITileMap map)
        {
            _random = random;
            _map = map;

            GridSize = 6;
        }

        public void Execute()
        {
            _roomGrid = new Room[_map.Width/GridSize, _map.Height/GridSize];
            var location = new Point {
                X = _roomGrid.GetLength(0)/2,
                Y = _roomGrid.GetLength(1)/2
            };

            var centerRoom = _roomGrid[location.X, location.Y] = new Room { 
                Location = location,
                Type = RoomType.Room
            };

            var unprocessed = new Queue<Room>();
            unprocessed.Enqueue(centerRoom);

            var directions = DirectionHelpers.Values();

            var totalCount = 1;

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

                        var canSpawnRoomAtLocation = _map.Contains(newRoomLocation, GridSize) 
                                                  && _roomGrid[newRoomLocation.X, newRoomLocation.Y] == default(Room);
                        var shouldSpawn = Chance(75 - totalCount);

                        // can spawn if its a valid location and there is a chance to spawn said room
                        return shouldSpawn && canSpawnRoomAtLocation;
                    })
                    // spawn rooms in those directions
                    .Select(direction =>
                    {
                        var newLocation = room.Location.Move(direction);
                        _map.Carve(room.GetCenterWallPoint(direction, GridSize), 1, 1, 1);
                        totalCount++;
                        return _roomGrid[newLocation.X, newLocation.Y] = Room.CreateRoom(newLocation, direction);
                    });


                newRooms.Aggregate(unprocessed, (acc, spawnedRoom) => {
                    // add them to the unprocessed list
                    acc.Enqueue(spawnedRoom);
                    return acc;
                });

                // carve our room
                room.Carve(_map, GridSize);
            }
        }

        private bool Chance(int chance)
        {
            return _random.Next(0, 101) <= chance;
        }

        public int GridSize { get; set; }
    }
}