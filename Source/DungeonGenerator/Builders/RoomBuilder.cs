using DungeonGenerator.Builders.Genetics;

namespace DungeonGenerator.Builders
{
    public class RoomBuilder : Builder
    {
        public RoomBuilder(int x, int y, Gene gene = new Gene()) : base(x, y, gene)
        {
        }



    }
}
