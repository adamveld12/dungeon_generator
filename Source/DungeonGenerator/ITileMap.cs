namespace Dungeon.Generator
{
    /// <summary>
    /// A container abstraction for level tile information
    /// </summary>
    public interface ITileMap
    {
        /// <summary>
        /// Indexes the tile information
        /// </summary>
        /// <param name="x">The x coord</param>
        /// <param name="y">The y coord</param>
        Tile this[int x, int y] { get; set; }

        /// <summary>
        /// Gets the width of the tile information
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the height of the tile information
        /// </summary>
        int Height { get; }
    }
}
