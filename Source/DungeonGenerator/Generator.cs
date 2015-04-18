namespace Dungeon.Generator
{
    /// <summary>
    /// Facade for generating dungeon like levels
    /// </summary>
    public static class Generator
    {
        /// <summary>
        /// Generates a dungeon
        /// </summary>
        /// <param name="size">The desired size</param>
        /// <param name="parameters">Generation options</param>
        /// <returns>An <see cref="ITileMap"/>, a 2D array of tile information</returns>
        public static ITileMap Generate(MapSize size, GeneratorParams parameters)
        {
            var map = new TileMap((int) size, (int) size);
            new CellBasedGenerator(parameters).Generate(map);
            return map;
        }

        /// <summary>
        /// Generates a dungeon, and allows for an optional seed
        /// </summary>
        /// <param name="size">The desired size</param>
        /// <param name="seed">Generation seed</param>
        /// <returns>An <see cref="ITileMap"/>, a 2D array of tile information</returns>
        public static ITileMap Generate(MapSize size, uint seed)
        {
            var map = new TileMap((int) size, (int) size);

            var parameters = GeneratorParams.Default;
            parameters.Seed = seed;

            new CellBasedGenerator(parameters).Generate(map);

            return map;
        }

        /// <summary>
        /// Generates a dungeon
        /// </summary>
        /// <param name="width">The desired width</param>
        /// <param name="height">The desired height</param>
        /// <param name="seed">Generation seed</param>
        /// <returns>An <see cref="ITileMap"/>, a 2D array of tile information</returns>
        public static ITileMap Generate(int width, int height, uint seed)
        {
            var map = new TileMap(width, height);

            var parameters = GeneratorParams.Default;
            parameters.Seed = seed;

            new CellBasedGenerator(parameters).Generate(map);

            return map;
        }
    }
}