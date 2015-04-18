namespace Dungeon.Generator
{
    public struct GeneratorParams
    {
        public float RoomChance { get; set; }
        public float Doors { get; set; }
        public float  MonsterSpawns { get; set; }
        public bool Exits { get; set; }
        public float Loot { get; set; }
        public uint Seed { get; set; }

        public static GeneratorParams Default
        {
            get
            {
                return new GeneratorParams {
                    Seed = 1024,
                    Doors = 1.0f,
                    Exits = true,
                    Loot = .25f,
                    MonsterSpawns = .66f,
                    RoomChance = .66f
                };
            }
        }
    }
}