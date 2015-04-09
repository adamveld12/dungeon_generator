namespace Dungeon.Generator
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum MapSize
    {
        Small = 5 * DungeonGenerator.CellSize,
        Medium = 11 * DungeonGenerator.CellSize,
        Large = 17 * DungeonGenerator.CellSize,
        Huge = 23 * DungeonGenerator.CellSize,
        Gargantuan = 31 * DungeonGenerator.CellSize,
        Biblical = 63 * DungeonGenerator.CellSize
    }
}