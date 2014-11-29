using System;
using System.Collections.Generic;
using DungeonGenerator.Navigation;

namespace DungeonGenerator.Generation.Generators
{
    public class RoomFirstGeneratorStrategy : IDungeonGenerationStrategy
    {

        public void Execute(MersennePrimeRandom random, ITileMap map)
        {
            // layout rooms
            var rooms = GenerateRooms(map);

            PlaceItems(map, rooms);
        }


        private IEnumerable<Room> GenerateRooms(ITileMap map)
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


            // connect rooms
            // for each room
                // for each wall
                    // randomly select a 'feature' to place on that side
                    // if the feature fits
                        // place it
                        // add the feature to the unvisited feature list
                    // else
                        // look for another feature
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

        private void PlaceItems(ITileMap map, IEnumerable<Room> rooms)
        {
            // place entrance at a random point in the center room
            // place exit in a random room near one of the corners
            // place boss spawns at corners of map
        }
    }
}