using System.Collections.Generic;
using System.Linq;
using Dungeon.Generator.Navigation;

namespace Dungeon.Generator.Generation.Generators
{
    public class RoomFirstGeneratorStrategy : IDungeonGenerationStrategy
    {
        public void Execute(MersennePrimeRandom random, ITileMap map)
        {
            // layout rooms
            var rooms = GenerateRooms(map, random);

            // connect rooms
            /* var hallways = */ ConnectRooms(map, rooms, random);

            // place some items in the rooms
            var items = PlaceItems(map, rooms, random);

            // return new MapMeta(rooms, hallways, items);
        }

        /// <summary>
        /// Using a random, calculates a 'chance' or percentage of an event happening, from 0 - 100
        /// </summary>
        /// <param name="random"></param>
        /// <param name="chance"></param>
        /// <returns></returns>
        public bool Chance(MersennePrimeRandom random, int chance)
        {
            return random.Next(0, 101) <= chance;
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

            var roomList = new List<Room> {
                // first room in the list will be the center room
                centerRoom
            };

            var width = map.Width;
            var height = map.Height;

            const int roomSize = 4;


            /**
             * generate and place rooms
             * 
             * create a grid over the tile map
             * for each grid location
             *   60% chance to place a room
             *   pick random width/height
             **/

            return roomList;
        }

        private void ConnectRooms(ITileMap map, IEnumerable<Room> rooms, MersennePrimeRandom random)
        {
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


        private IEnumerable<Item> PlaceItems(ITileMap map, IEnumerable<Room> rooms, MersennePrimeRandom random)
        {
            
            // place entrance at a random point in the center room
            rooms.First().PlaceItem(Item.Entrance);

            var width = map.Width;
            var height = map.Height;

            // place exit in a random room near one of the corners
            var exitRooms = rooms.Where(room => {
                var inLeft = room.X <= width*0.20;
                var inTop = room.Y <= height*0.20;
                var inRight = room.X >= (width - width*0.20);
                var inBottom = room.Y >= (height - height*0.20);

                return (inLeft && inTop 
                    || inLeft && inBottom 
                    || inRight && inTop 
                    || inRight && inBottom);

            }).ToList();

            var items = new List<Item>(32);

            // place exit in a random exit
            var randomIndex = random.Next(exitRooms.Count);
            items.Add(
                exitRooms[randomIndex].PlaceItem(Item.Exit)
            );

            // place boss and loot spawns at corners of map
            exitRooms.ForEach(x => {
                items.Add(
                    x.PlaceItem(Item.BossSpawn)
                );
                items.Add(
                    x.PlaceItem(Item.LootSpawn)
                );
            });

            return items;
        }
    }
}