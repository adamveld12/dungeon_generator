namespace Dungeon.Generator.Navigation
{
    public struct Item
    {
        public Point Location;
        public byte Type;

        public static Item Entrance = new Item { Type = 0x00 };
        public static Item Exit = new Item { Type = 0x01 };
        public static Item BossSpawn = new Item { Type = 0x02 };
        public static Item LootSpawn = new Item { Type = 0x03 };
    }
}