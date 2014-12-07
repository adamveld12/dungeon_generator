namespace Dungeon.Generator.Generation.Generators.RoomBased
{
    internal class Room
    {
        public RoomType Type { get; set; }
        public Point Size { get; set; }
        public Point Location { get; set; }
        public Direction Cardinality { get; set; }
 
        public static Room CreateCorridor(Point location, Direction direction, int length)
        {
            return new Room {
                Location = location,
                Size = direction.Normal() * length,
                Type = RoomType.Corridor,
                Cardinality = direction
            };
        }

        public static Room CreateRoom(Point location, Direction direction)
        {
            return new Room {
                Type = RoomType.Room,
                Cardinality = direction,
                Location = location
            };
        }

    }
}