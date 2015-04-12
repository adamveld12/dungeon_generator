using System.Diagnostics;

namespace Dungeon.Generator
{
    [DebuggerDisplay("({X},{Y})")]
    internal struct Point
    {
        public int X { get; set; }  
        public int Y { get; set; }  
    }
}