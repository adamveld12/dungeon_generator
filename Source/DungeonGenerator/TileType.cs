using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Dungeon.Generator
{
    [DebuggerDisplay("Type: {Type}")]
    [StructLayout(LayoutKind.Explicit)]
    public struct TileType
    {
        [FieldOffset(0)]
        public byte Type;

        /// <summary>
        /// For things like doors.. maybe 
        /// </summary>
        [FieldOffset(1)]
        public byte Attributes;

        public const byte Air = 0x00;
        public const byte Floor = 0x01;
        public const byte Wall = 0x02;
        public const byte BreakableWall = 0x03;
    }
}