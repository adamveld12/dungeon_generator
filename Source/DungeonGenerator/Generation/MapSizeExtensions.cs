using System;

namespace Dungeon.Generator.Generation
{
    internal struct Dimension
    {
        public int X;
        public int Y;
    }
    public static class MapSizeExtensions
    {
        public static Dimension ToDimensions(this MapSize mapSize)
        {
            Dimension dimensions;

            switch (mapSize)
            {
                case MapSize.Tiny:
                    dimensions = new Dimension { 
                        X = 61,
                        Y = 61
                    };
                    break;
                case MapSize.Small:
                    dimensions = new Dimension { 
                        X = 121,
                        Y = 121 
                    };
                    break;
                case MapSize.Medium:
                    dimensions = new Dimension { 
                        X = 241,
                        Y = 241 
                    };
                    break;
                case MapSize.Large:
                    dimensions = new Dimension { 
                        X = 1201,
                        Y = 1201 
                    };
                    break;
                case MapSize.Enormous:
                    dimensions = new Dimension { 
                        X = 1801,
                        Y = 1801 
                    };
                    break;
                case MapSize.Biblical:
                    dimensions = new Dimension { 
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