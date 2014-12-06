using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
            // generate features on the map in grid space


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
             **/
            // place a room in the center
            var mapCenter = _dimensions/2;
            var centerRoom = new Feature
            {
                Location = mapCenter,
                Direction = Direction.N,
                Type = FeatureType.Room
            };

            _features[mapCenter.X, mapCenter.Y] = centerRoom;


            var corridorTypes = Enum.GetValues(typeof (CorridorType)).Cast<CorridorType?>();
            // add to unprocessed
            var unprocessed = new Queue<Feature>(1024);
            unprocessed.Enqueue(centerRoom);

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
                        .Where(wallDirection => featureLocation.CanMove(wallDirection, _features))
                        .Where(wallDirection => {
                            var newLocation = featureLocation.Move(wallDirection);
                            return _features[newLocation.X, newLocation.Y].Type == FeatureType.None;
                        })
                        .Where(x => Chance(75))
                        .Select(wallDirection => {
                            // carve outlet
                            var outletLocation = featureLocation.GetCenterWallPoint(wallDirection, GridSize);
                            _map.Carve(outletLocation, 1, 1, 1);

                            // move to that location
                            var newLocation = featureLocation.Move(wallDirection);

                            var newFeature = new Feature {
                                Direction = wallDirection,
                                Location = newLocation
                            };

                            // a 65% chance to spawn a room
                            if (Chance(65)) newFeature.Type = FeatureType.Room;
                                // a 33% chance to spawn a corridor
                            else
                            {
                                newFeature.Type = FeatureType.Corridor;
                                newFeature.CorridorType = corridorTypes.FirstOrDefault(x => Chance(10)) ?? CorridorType.OneWayCorridor;
                            }

                            // set the feature in our datastructure
                            _features[newLocation.X, newLocation.Y] = newFeature;

                            return newFeature;
                        })
                        .Aggregate(unprocessed, (acc, newFeature) => {
                            acc.Enqueue(newFeature);
                            return acc;
                        });
                    //feature.CarveRoom(_map, GridSize);
                }
                // else if corridor
                else if (feature.Type == FeatureType.Corridor)
                {

                    // 100% chance of it being a one way
                    feature.CorridorType.Walls(feature.Direction)
                        // maybe check if all directions are clear for movement, and if not pick a different corridor type
                        .Where(newDirection => featureLocation.CanMove(newDirection, _features))
                        .Where(wallDirection => {
                            var newLocation = featureLocation.Move(wallDirection);
                            return _features[newLocation.X, newLocation.Y].Type == FeatureType.None;
                        })
                        .Select(direction => {
                            var newLocation = featureLocation.Move(direction);
                            var newFeature = new Feature
                            {
                                Location = newLocation,
                                Direction = direction
                            };

                            // 20% chance to spawn another corridor
                            if (Chance(20))
                            {
                                newFeature.Type = FeatureType.Corridor;
                                newFeature.CorridorType = corridorTypes.FirstOrDefault(x => Chance(10)) ?? CorridorType.OneWayCorridor;
                            }
                            // otherwise spawn a room
                            else newFeature.Type = FeatureType.Room;

                            // set the feature in our datastructure
                             _features[newLocation.X, newLocation.Y] = newFeature;
                            
                            return newFeature;
                        })
                        .Aggregate(unprocessed, (acc, newFeature) =>
                        {
                            acc.Enqueue(newFeature);
                            return acc;
                        });
                }
                
            } while (unprocessed.Count > 0);

            // STEP 2
            // make sure corridors are connected properly
            // place doors and items
            // generate anterooms 
            foreach (var feature in _features)
            {
                var location = feature.Location;
                var surroundingFeatures = Surrounding(feature);
                var type = feature.Type;

                if(type == FeatureType.Room)
                    feature.CarveRoom(_map, GridSize);
                else if (type == FeatureType.Corridor)
                    feature.CarveCorridor(_map, GridSize);

            }
        }

        private IEnumerable<Feature> Surrounding(Feature feature)
        {
            var location = feature.Location;
            return DirectionHelpers.Values().Where(x => location.CanMove(x, _features))
                .Where(x => {
                    var newLocation = location.Move(x);
                    var directionalFeature = _features[newLocation.X, newLocation.Y];
                    return directionalFeature.Type != FeatureType.None;
                })
                .Select(x =>
                {
                    var newLocation = location.Move(x);
                    return _features[newLocation.X, newLocation.Y];
                });
        }

        public bool Chance(int chance)
        { return _random.Next(0, 101) <= chance; }

        public int GridSize { get; set; }
    }
}