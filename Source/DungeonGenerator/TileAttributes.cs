using System;

namespace Dungeon.Generator
{
    [Flags]
    public enum TileAttributes : ushort
    {
        Air = 0x0000,
        Floor = 0x0001,
        Wall = 0x0002,
        BreakableWall = 0x0003,

        None = 0x0000,
        Exit = 0x0100,
        Loot = 0x0200,
        MonsterSpawn = 0x0400,
        Doors = 0x0800,
        Unlit_Attribute = 0x0000,
        Shadowed_Attribute = 0x1000,
        Illuminated_Attribute = 0x2000,
    }
}