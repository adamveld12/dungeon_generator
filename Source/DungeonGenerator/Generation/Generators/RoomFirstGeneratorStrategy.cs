using DungeonGenerator.Navigation;

namespace DungeonGenerator.Generation.Generators
{
    public class RoomFirstGeneratorStrategy : IDungeonGenerationStrategy
    {
        public void Execute(ITileMap map)
        {
            // layout rooms
            // place a room in the center of the map
                // for each wall
                    // randomly select a 'feature' to place on that side
                    // if the feature fits
                        // place it
                        // add the feature to the unvisited feature list
                    // else
                        // look for another feature

            // place entrance at a random point in the center room
            // place exit in a random room near one of the corners
        }
    }
}