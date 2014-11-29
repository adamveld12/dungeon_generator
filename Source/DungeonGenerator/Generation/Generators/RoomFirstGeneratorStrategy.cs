using System.Collections.Generic;
using System.Linq;
using DungeonGenerator.Navigation;

namespace DungeonGenerator.Generation.Generators
{
    public class RoomFirstGeneratorStrategy : IDungeonGenerationStrategy
    {
        public void Execute(MersennePrimeRandom random, ITileMap map)
        {
            // layout rooms
            var rooms = GenerateRooms(map, random);

            // connect rooms
            ConnectRooms(map, rooms, random);

            // place some items in the rooms
            var items = PlaceItems(map, rooms, random);
        }


        public bool Chance(int chance)
        {
            return false;
        }

        private void ConnectRooms(ITileMap map, IEnumerable<Room> rooms, MersennePrimeRandom random)
        {
            
             /*
              * connect rooms
              * for each room
              * for each wall
              *     randomly select a 'feature' to place on that side
              *     if the feature fits
              *       place it
              *       add the feature to the unvisited feature list
              *      else
              *        look for another feature
              */
        }

        private IEnumerable<Room> GenerateRooms(ITileMap map, MersennePrimeRandom random)
        {
            // place a room in the center of the map
            const int centerRoomSize = 4;
            var centerRoom = new Room
            {
                Height = centerRoomSize,
                Width = centerRoomSize,
                X =  map.Width/2 - centerRoomSize/2,
                Y = map.Height/2 - centerRoomSize/2
            };

            // generate and place rooms
            // create a grid over the tile map

            // for each grid location
                // 60% chance to place a room
                // pick random width/height

            /* 
            * Paths: Connects rooms together
            * 
            * Hallway
            *  A hallway that is 1 unit wide and turns,
            *  If the length is greater than 3 before and after a turn, add an anteroom
            *
            * Great Hall
            *  A hallway that is 3 units wide and doesn't turn
            * 
            */
            

        }

        private void PlaceItems(ITileMap map, IEnumerable<Room> rooms, MersennePrimeRandom random)
        {
            // place entrance at a random point in the center room
            rooms.First().PlaceItem(Item.Entrance);

            var width = map.Width;
            var height = map.Height;

            // place exit in a random room near one of the corners
            var exitRooms = rooms.Where(room =>
            {
                var inLeft = room.X <= width*0.20;
                var inTop = room.Y <= height*0.20;
                var inRight = room.X >= (width - width*0.20);
                var inBottom = room.Y >= (height - height*0.20);

                return (inLeft && inTop 
                    || inLeft && inBottom 
                    || inRight && inTop 
                    || inRight && inBottom);

            }).ToList();

            // place exit in a random exit
            var randomIndex = random.Next(exitRooms.Count);
            exitRooms[randomIndex].PlaceItem(Item.Exit);

            // place boss and loot spawns at corners of map
            exitRooms.ForEach(x => {
                x.PlaceItem(Item.BossSpawn);
                x.PlaceItem(Item.LootSpawn);
            });
        }
    }
}