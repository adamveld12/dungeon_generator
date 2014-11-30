using System;
using System.Collections.Generic;
using System.Linq;
using Dungeon.Generator.Navigation;

namespace Dungeon.Generator.Generation.Generators
{
    public class RoomFirstGeneratorStrategy : IDungeonGenerationStrategy
    {
        public RoomFirstGeneratorStrategy()
        {
            MaxRoomSize = 6;
            RoomChance = 80;
            CornerSize = 0.20;
            CenterRoomSize = 8;
        }

        public void Execute(MersennePrimeRandom random, ITileMap map)
        {
            // layout rooms
            var rooms = GenerateRooms(map, random);

            // connect rooms
            var hallways = ConnectRooms(map, rooms, random);

            // place some items in the rooms
            var items = PlaceItems(map, rooms, random);

            // return new MapMeta(rooms, hallways, items);
        }

        #region Private Parts 

        private IEnumerable<Room> GenerateRooms(ITileMap map, MersennePrimeRandom random)
        {
            // place a room in the center of the map
            var centerRoom = new Room
            {
                Height = CenterRoomSize,
                Width = CenterRoomSize,
                X = map.Width/2 - CenterRoomSize/2,
                Y = map.Height/2 - CenterRoomSize/2
            };

            var roomList = new List<Room> {
                // first room in the list will be the center room
                centerRoom
            };

            // generate and place rooms
            var width = map.Width;
            var height = map.Height;

            // create a grid over the tile map
            // for each grid location...
            for (var gridX = 0; gridX < width/MaxRoomSize; gridX++)
                for (var gridY = 0; gridY < height/MaxRoomSize; gridY++)
                    // if there is a chance to place a room
                    if (Chance(random, RoomChance))
                    {
                        // use a random width and height
                        var roomWidth = random.Next(2, MaxRoomSize);
                        var roomHeight = random.Next(2, MaxRoomSize);

                        // create a new room with random width and height
                        // at the room location
                        var room = new Room {
                            Width = roomWidth,
                            Height = roomHeight,
                            X = gridX * MaxRoomSize + 1,
                            Y = gridY * MaxRoomSize + 1
                        };

                        // add the room
                        roomList.Add(room);
                    }

            // carve all the rooms
            // this makes us iterate over the room list again giving us O(2N)
            roomList.ForEach(room => MapEditorTools.CarveRoom(map, room));

            return roomList;
        }

        private IEnumerable<Pathway> ConnectRooms(ITileMap map, IEnumerable<Room> rooms, MersennePrimeRandom random)
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

            var pathways = Enumerable.Empty<Pathway>();

            var directions = Enum.GetValues(typeof (Direction)).Cast<Direction>();

            // for each room get the center wall points
            rooms.SelectMany(room => directions.Select(room.GetCenterWallPoint))
                 .Select(wallPoint => 1);

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

            return pathways;
        }

        private IEnumerable<Item> PlaceItems(ITileMap map, IEnumerable<Room> rooms, MersennePrimeRandom random)
        {
            // place entrance at a random point in the center room
            rooms.First().PlaceItem(Item.Entrance);

            var width = map.Width;
            var height = map.Height;

            // place exit in a random room near one of the corners
            var exitRooms = rooms.Where(room => {
                var inLeft = room.X <= width*CornerSize;
                var inTop = room.Y <= height*CornerSize;
                var inRight = room.X >= (width - width*CornerSize);
                var inBottom = room.Y >= (height - height*CornerSize);

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

        /// <summary>
        /// Using a random, calculates a 'chance' or percentage of an event happening, from 0 - 100
        /// </summary>
        /// <param name="random"></param>
        /// <param name="chance"></param>
        /// <returns></returns>
        private bool Chance(MersennePrimeRandom random, int chance)
        { return random.Next(0, 101) <= chance; }

        #endregion

        #region Properties

        public int RoomChance { get; set; }
        public double CornerSize { get; set; }
        public int MaxRoomSize { get; set; }
        public int CenterRoomSize { get; set; }

        #endregion
    }
}