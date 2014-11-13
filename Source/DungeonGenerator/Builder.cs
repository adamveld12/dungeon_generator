using System;
using System.Collections.Generic;
using DungeonGenerator.Navigation;

namespace DungeonGenerator
{
    public abstract class Builder
    {
        protected Location _location;

        public abstract void Build(ITileMap map);
        public abstract IEnumerable<Builder> Spawn();

        /// <summary>
        /// Carves out a section of the dungeon map using the _position as the origin
        /// </summary>
        /// <param name="map"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="type"></param>
        protected void Carve(ITileMap map, int width, int height, TileType type)
        {
            var widthUpper = _location.X + width;
            var heightUpper = _location.Y + height;
            if (widthUpper < map.Width && heightUpper < map.Height && widthUpper >= 0 && heightUpper >= 0)
            {
              for (var x = _location.X; x < widthUpper; x++)
                  for (var y = _location.Y; y < heightUpper; y++)
                      map[x, y] = type;
            }
            else
                throw new IndexOutOfRangeException("Could not carve because dimensions did not fit tile map");
        }

        public Location Location
        {
            get { return _location; }
            set { _location = value; }
        }
    }

    public class HallwayBuilder : Builder
    {
        public override void Build(ITileMap map)
        {
        }

        public override IEnumerable<Builder> Spawn()
        {
            // spawns anterooms at its turns
            yield break;
        }

//        public Room Target { get; set; }
    }

    public class RoomBuilder : Builder
    {
        public RoomBuilder()
        {
            Height = Width = 1;
        }

        public override void Build(ITileMap map)
        {
            Carve(map, Width, Height, TileType.Floor);
        }

        public override IEnumerable<Builder> Spawn()
        {
            // generates doors
            // which go on to carve hall ways
            yield break;
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public int DoorCount { get; set; }
    }
}