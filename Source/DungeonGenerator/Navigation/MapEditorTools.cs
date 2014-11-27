using System;
using DungeonGenerator.Generation;

namespace DungeonGenerator.Navigation
{
    public static class MapEditorTools
    {
        public static Point ToPoint(MapSize size)
        {
            var dimensions = new Point();
            switch (size)
            {
                case MapSize.Tiny:
                    dimensions.Y = dimensions.X = 64;
                    break;
                case MapSize.Small:
                    dimensions.Y = dimensions.X = 128;
                    break;
                case MapSize.Medium:
                    dimensions.Y = dimensions.X = 256;
                    break;
                case MapSize.Large:
                    dimensions.Y = dimensions.X = 768;
                    break;
                // uses about 8 megs of ram
                case MapSize.Enormous:
                    dimensions.Y = dimensions.X = 2048;
                    break;
                // uses about 126 megs of ram
                case MapSize.Biblical:
                    dimensions.Y = dimensions.X = 8128;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("size");
            }

            return dimensions;
        }

        public static bool Carve(ITileMap map, Point startLocation, int width, int height, ushort type)
        {
            var widthUpper = startLocation.X + width;
            var heightUpper = startLocation.Y + height;
            var canCarve = false;

            if (widthUpper < map.Width && heightUpper < map.Height && widthUpper >= 0 && heightUpper >= 0)
            {
                canCarve = true;
                for (var x = startLocation.X; x < widthUpper; x++)
                    for (var y = startLocation.Y; y < heightUpper; y++)
                        map[x, y] = type;
            }

            return canCarve;
        }
    }
}