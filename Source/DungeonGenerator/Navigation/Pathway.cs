namespace Dungeon.Generator.Navigation
{
    public struct Pathway
    {
        public Direction Direction;
        public Room Start;
        public Room End;

        public void Carve(ITileMap map)
        {
            var startLocation = Start.Center;
            var endLocation = End.Center;
            var distance = new Point(endLocation.X - startLocation.X, endLocation.Y - startLocation.Y);

            MapEditorTools.Carve(map, startLocation, distance.Y, 1, 1);
            MapEditorTools.Carve(map, endLocation, distance.Y, 1, 1);

        }
    }
}