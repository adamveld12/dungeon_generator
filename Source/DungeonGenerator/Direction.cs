namespace Dungeon.Generator
{
    internal enum Direction : byte
    {
        N = 0, 
        E, 
        S, 
        W,
        Max = W,
        Min = N
    }
}