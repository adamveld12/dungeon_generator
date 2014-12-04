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
                        X = 61,
                        Y = 61
                    };
                    break;
                case MapSize.Small:
                    dimensions = new Point { 
                        X = 121,
                        Y = 121 
                    };
                    break;
                case MapSize.Medium:
                    dimensions = new Point { 
                        X = 241,
                        Y = 241 
                    };
                    break;
                case MapSize.Large:
                    dimensions = new Point { 
                        X = 1201,
                        Y = 1201 
                    };
                    break;
                case MapSize.Enormous:
                    dimensions = new Point { 
                        X = 1801,
                        Y = 1801 
                    };
                    break;
                case MapSize.Biblical:
                    dimensions = new Point { 
                        X = 8401,
                        Y = 8401
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mapSize");
            }

            return dimensions;
        }
    }
}