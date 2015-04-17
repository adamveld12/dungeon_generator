namespace Dungeon.Generator
{
    public struct GeneratorParams
    {
        public float RoomChance { get; set; }
        public bool Doors { get; set; }
        public bool  MonsterSpawns { get; set; }
        public bool Exits { get; set; }
        public bool Loot { get; set; }
        public uint Seed { get; set; }

        public static GeneratorParams Default
        {
            get
            {
                return new GeneratorParams {
                    Seed = 1024,
                    Doors = true,
                    Exits = true,
                    Loot = true,
                    MonsterSpawns = true,
                    RoomChance = .66f
                };
            }
        }
    }
}