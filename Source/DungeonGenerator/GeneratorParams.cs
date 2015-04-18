namespace Dungeon.Generator
{
    /// <summary>
    /// Dungeon generation options
    /// </summary>
    public struct GeneratorParams
    {
        /// <summary>
        /// Get or set the percentage of rooms to corridors. Values are 0 - 1.0f
        /// </summary>
        public float RoomChance { get; set; }

        /// <summary>
        /// Get or set the percentage of doors to create. Values are 0 - 1.0f
        /// </summary>
        public float Doors { get; set; }

        /// <summary>
        /// Get or set the percentage of Mob spawns to create. Values are 0 - 1.0f
        /// </summary>
        public float MobSpawns { get; set; }

        /// <summary>
        /// Gets or sets if the generator should place an Entry and Exit tile.
        /// </summary>
        public bool Exits { get; set; }

        /// <summary>
        /// Gets or sets the percentage of Loot rooms to create. Values are 0 - 1.0f
        /// </summary>
        public float Loot { get; set; }

        /// <summary>
        /// Gets or sets if Mobs should only be placed in rooms. 
        /// </summary>
        public bool MobsInRoomsOnly { get; set; }

        /// <summary>
        /// Gets or sets the Seed for the generator.
        /// </summary>
        public uint Seed { get; set; }

        /// <summary>
        /// Gets a <see cref="GeneratorParams"/> configured with sensible defaults
        /// </summary>
        public static GeneratorParams Default
        {
            get
            {
                return new GeneratorParams {
                    Seed = 1024,
                    Doors = 1.0f,
                    Exits = true,
                    Loot = .25f,
                    MobSpawns = .66f,
                    RoomChance = .66f,
                    MobsInRoomsOnly = false,

                };
            }
        }
    }
}