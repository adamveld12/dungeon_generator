using System.Collections.Generic;
using Dungeon.Generator.Navigation;

namespace Dungeon.Generator.Generation.Generators
{
    public class MapMeta
    {
        private readonly Room[] _rooms;
        private readonly Pathway[] _hallways;
        private readonly Item[] _items;

        public MapMeta(Room[] rooms, Pathway[] hallways, Item[] items)
        {
            _rooms = rooms;
            _hallways = hallways;
            _items = items;
        }

        public IEnumerable<Room> Rooms
        {
            get { return _rooms; }
        }

        public IEnumerable<Pathway> Hallways
        {
            get { return _hallways; }
        }

        public IEnumerable<Item> Items
        {
            get { return _items; }
        }
    }
}