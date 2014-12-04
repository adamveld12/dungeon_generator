using System;

namespace Dungeon.Generator.Generation
{
    public enum MapSize : byte
    {
        Tiny = 0,
        Small,
        Medium,
        Large,
        Enormous,
        Biblical,
    }

    public static class MapSizeExtensions
    {
        public static Point ToDimensions(this MapSize mapSize)
        {
            Point dimensions;

            switch (mapSize)
            {
                case MapSize.Tiny:
                    dimensions = new Point { 
                        X = 60,
                        Y = 60
                    };
                    break;
                case MapSize.Small:
                    dimensions = new Point { 
                        X = 120,
                        Y = 120 
                    };
                    break;
                case MapSize.Medium:
                    dimensions = new Point { 
                        X = 240,
                        Y = 240 
                    };
                    break;
                case MapSize.Large:
                    dimensions = new Point { 
                        X = 1200,
                        Y = 1200 
                    };
                    break;
                case MapSize.Enormous:
                    dimensions = new Point { 
                        X = 1800,
                        Y = 1800 
                    };
                    break;
                case MapSize.Biblical:
                    dimensions = new Point { 
                        X = 8400,
                        Y = 8400
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mapSize");
            }

            return dimensions;
        }
    }
}