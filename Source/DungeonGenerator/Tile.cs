using System;
using System.Runtime.InteropServices;

namespace Dungeon.Generator
{
    /// <summary>
    /// A representation of a tile
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Tile
    {
        /// <summary>
        /// The material type of this tile
        /// </summary>
        [FieldOffset(0)]
        public MaterialType MaterialType;

        /// <summary>
        /// The meta data info about this tile
        /// </summary>
        [FieldOffset(1)]
        public AttributeType Attributes;

        /// <summary>
        /// A <see cref="Tile"/> with a floor <see cref="MaterialType"/> 
        /// </summary>
        public static readonly Tile Floor = new Tile {MaterialType = MaterialType.Floor};

        /// <summary>
        /// A <see cref="Tile"/> with a Wall <see cref="MaterialType"/> 
        /// </summary>
        public static readonly Tile Wall = new Tile {MaterialType = MaterialType.Wall};

        /// <summary>
        /// A <see cref="Tile"/> with an Air <see cref="MaterialType"/> 
        /// </summary>
        public static readonly Tile Air = new Tile {MaterialType = MaterialType.Air};
    }

    /// <summary>
    /// The material of the tile
    /// </summary>
    public enum MaterialType : byte
    {
        /// <summary>
        /// Nothing
        /// </summary>
        Air = 0x00,
        /// <summary>
        /// Traversable ground
        /// </summary>
        Floor,
        /// <summary>
        /// An impassable, indestructible wall
        /// </summary>
        Wall,
        /// <summary>
        /// A destructible will
        /// </summary>
        BreakableWall,
    }

    /// <summary>
    /// Tile meta data
    /// </summary>
    [Flags]
    public enum AttributeType : byte
    {
        /// <summary>
        /// Nothing
        /// </summary>
        None = 0x00,
        /// <summary>
        /// An entrance to the dungeon
        /// </summary>
        Entry = 0x01,
        /// <summary>
        /// An exit to the dungeon
        /// </summary>
        Exit = 0x02,
        /// <summary>
        /// A loot/treasure spawn
        /// </summary>
        Loot = 0x04,
        /// <summary>
        /// A mob/AI spawn
        /// </summary>
        MobSpawn = 0x08,
        /// <summary>
        /// A bi directional doorway
        /// </summary>
        Doors = 0x10
    }
}