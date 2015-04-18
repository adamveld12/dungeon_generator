using System;
using System.Runtime.InteropServices;

namespace Dungeon.Generator
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Tile
    {
        [FieldOffset(0)]
        public TileMaterial Material;

        [FieldOffset(1)]
        public TileAttributes Attributes;

        public static readonly Tile Floor = new Tile {Material = TileMaterial.Floor};
        public static readonly Tile Wall = new Tile {Material = TileMaterial.Wall};
        public static readonly Tile Air = new Tile {Material = TileMaterial.Air};

    }

    public enum TileMaterial : byte
    {
        Air = 0x00,
        Floor,
        Wall,
        BreakableWall,
    }

    [Flags]
    public enum TileAttributes : byte
    {
        None = 0x00,
        Entry = 0x01,
        Exit = 0x02,
        Loot = 0x04,
        MonsterSpawn = 0x08,
        Doors = 0x10
    }
}