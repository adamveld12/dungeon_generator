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
            var dimensions = Point.Zero;

            switch (mapSize)
            {
                case MapSize.Tiny:
                    dimensions = new Point { 
                        X = 64,
                        Y = 64
                    };
                    break;
                case MapSize.Small:
                    dimensions = new Point { 
                        X = 128,
                        Y = 128 
                    };
                    break;
                case MapSize.Medium:
                    dimensions = new Point { 
                        X = 256,
                        Y = 256 
                    };
                    break;
                case MapSize.Large:
                    dimensions = new Point { 
                        X = 1024,
                        Y = 1024 
                    };
                    break;
                case MapSize.Enormous:
                    dimensions = new Point { 
                        X = 2048,
                        Y = 2048 
                    };
                    break;
                case MapSize.Biblical:
                    dimensions = new Point { 
                        X = 8128,
                        Y = 8128
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mapSize");
            }

            return dimensions;
        }
    }
}