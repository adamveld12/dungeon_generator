using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Dungeon.Generator.Generation.Generators.RoomBased;
using Dungeon.Generator.Navigation;

namespace Dungeon.Generator.Generation.Generators.GridBased
{
    public class GridBased : IDungeonGenerationStrategy
    {
        private readonly MersennePrimeRandom _random;
        private readonly ITileMap _map;
        
        private readonly Point _dimensions;


        private readonly Feature[,] _features;

        public GridBased(MersennePrimeRandom random, ITileMap map)
        {
            _random = random;
            _map = map;

            GridSize = 6;
            _dimensions = new Point(map.Width, map.Height).ToGrid(GridSize);
        }

        public void Execute()
        {
            // place a room in the center
            var mapCenter = _dimensions/2;
            var center = _features[mapCenter.X, mapCenter.Y].Location = mapCenter;

            // add to unprocessed
            var unprocessed = new Queue<Feature>(1024);

            // for each unprocessed feature
            do
            {
                var feature = unprocessed.Dequeue();
                var featureLocation = feature.Location;

                // if room
                if (feature.Type == FeatureType.Room)
                {
                    // for each wall
                    feature.Walls()
                        // if the space is clear
                        .Where(wallDirection => featureLocation.CanMove(wallDirection, _map))
                        .Select(wallDirection => {
                            // move to that location
                            var newLocation = featureLocation.Move(wallDirection);
                            // a 65% chance to spawn a room
                            if (Chance(65))
                            {
                                return new Feature
                                {
                                    Direction = wallDirection,
                                    Location = newLocation,
                                    Type = FeatureType.Room
                                };
                            }
                            // a 33% chance to spawn a corridor
                            else if (Chance(33))
                            {
                                return new Feature {
                                    Direction = wallDirection,
                                    Location = newLocation,
                                    Type = FeatureType.Corridor
                                };

                            }
                            else return default(Feature);
                        });
                }
                // else if corridor
                else if (feature.Type == FeatureType.Corridor)
                {
                    // 100% chance of it being a one way

                }
            /**
             *  if room
             *    for each wall
             *    if space is clear
             *      a 65% chance to spawn a room
             *      a 33% chance to spawn a corridor
             *      add feature to unprocessed list
             *  else if corridor
             *    33% chance of being a four way
             *    33% chance of being a three way
             *    33% chance of it being a one way
             *      10% chance of turning either left or right
             *    50% chance of having anteroom
             *    for each wall
             *      a 20% chance of spawning a corridor
             *      if a corridor doesn't spawn, spawn a room
             *      add feature to unprocessed list
             *  carve myself into the map
             **/
            } while (unprocessed.Count > 0);
        }

        public bool Chance(int chance)
        { return _random.Next(0, 101) <= chance; }

        public int GridSize { get; set; }
    }
}