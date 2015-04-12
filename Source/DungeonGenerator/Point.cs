using System.Diagnostics;

namespace Dungeon.Generator
{
    [DebuggerDisplay("({X},{Y})")]
    public struct Point
    {
        public int X { get; set; }  
        public int Y { get; set; }  
    }
}