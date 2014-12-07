using System;
using System.Collections.Generic;
using System.Linq;
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
            _features = new Feature[_dimensions.X,_dimensions.Y];
        }

        public void Execute()
        {
            // STEP 1.
            // place a room in the center
            var mapCenter = _dimensions/2;
            var centerRoom = new Feature
            {
                Location = mapCenter,
                Type = FeatureType.Room
            };

            _features[mapCenter.X, mapCenter.Y] = centerRoom;


            var corridorTypes = Enum.GetValues(typeof (CorridorType)).Cast<CorridorType>().ToArray();
            // add to unprocessed
            var unprocessed = new Queue<Feature>(1024);
            unprocessed.Enqueue(centerRoom);

            // for each unprocessed feature
            do
            {
                // for each wall on the feature
                var feature = unprocessed.Dequeue();
                var featureLocation = feature.Location;

                feature.Connections = feature.Walls()
                    // 75% chance of spawning a feature from any wall
                    .Where(x => Chance(75))
                    // where we can move (on the map) and where there isn't a feature already in place
                    .Where(direction =>
                    {
                        var location = featureLocation.Move(direction);
                        return location.CanMove(direction, _features) && _features[location.X, location.Y] == null;
                    })
                    // project into new features
                    .Select(direction =>
                    {
                        var newLocation = featureLocation.Move(direction);
                        var newFeature = new Feature
                        {
                            Location = newLocation,
                            Origin = feature
                        };

                        // 66 percent chance of the new feature being a room
                        if (Chance(66))
                            newFeature.Type = FeatureType.Room;
                            // 33 percent chance of it being a corridor
                        else
                        {
                            newFeature.Type = FeatureType.Corridor;
                            // 20 percent chance for each corridor type
                            newFeature.CorridorType = corridorTypes.ElementAt(_random.Next(0, corridorTypes.Length));
                        }

                        return newFeature;
                    });

                    feature.Connections.Aggregate(unprocessed, (acc, newFeature) => {
                        // add to unprocessed list
                        acc.Enqueue(newFeature);
                        return acc;
                    });

                _features[featureLocation.X, featureLocation.Y] = feature;

            } while (unprocessed.Count > 0);

            // STEP 2
            // make sure corridors are connected properly
            // place doors and items
            // generate anterooms 
            foreach (var feature in _features)
            {
                if(feature == null)
                    continue;

                feature.Carve(_map, GridSize);
            }
        }

        public bool Chance(int chance)
        { return _random.Next(0, 101) <= chance; }

        public int GridSize { get; set; }
    }
}