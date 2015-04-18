namespace Dungeon.Generator
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum MapSize
    {
        Small = 5 * CellBasedGenerator.CellSize,
        Medium = 11 * CellBasedGenerator.CellSize,
        Large = 17 * CellBasedGenerator.CellSize,
        Huge = 23 * CellBasedGenerator.CellSize,
        Gargantuan = 31 * CellBasedGenerator.CellSize,
        Biblical = 63 * CellBasedGenerator.CellSize
    }
}