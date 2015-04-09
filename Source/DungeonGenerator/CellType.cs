using System;

namespace Dungeon.Generator
{
    public enum CellType : byte
    {
        None = 0,
        Room,
        Corridor,
    }

    public static class CellHelpers
    {
        public static void Fill(this Cell cell, int x, int y, ITileMap map)
        {
            // fills this cell's location with tiles
            TileType[,] template;

            // generate an array of values for the type
            switch (cell.Type)
            {
                case CellType.Room:
                    template = FillRoom(cell.Openings);
                    break;
                case CellType.Corridor:
                    template = FillCorridor(cell.Openings);
                    break;
                default: return;
            }

            var cellSize = DungeonGenerator.CellSize;

            // bake the template into the map
            for (var xPos = 0; xPos < template.GetLength(0); xPos++)
                for (var yPos = 0; yPos < template.GetLength(1); yPos++)
                    map[x * cellSize + xPos, y * cellSize + yPos] = template[xPos, yPos];

        }

        private static TileType[,] FillCorridor(Direction openings)
        {
            var template = new TileType[DungeonGenerator.CellSize, DungeonGenerator.CellSize];

            // fill half way on one axis, and halfway on the other
            foreach (var direction in openings.ToDirectionsArray())
            {
                // TODO dry this up
                if (direction == Direction.West)
                    for (int x = 0; x < template.GetLength(0)/2; x++)
                        template[x, DungeonGenerator.CellSize/2].Type = TileType.Floor;
                else if (direction == Direction.East)
                    for (int x = template.GetLength(0)/2; x < template.GetLength(0); x++)
                        template[x, DungeonGenerator.CellSize/2].Type = TileType.Floor;
                else if (direction == Direction.North )
                    for (int y = 0; y < template.GetLength(1)/2; y++)
                        template[DungeonGenerator.CellSize/2, y].Type = TileType.Floor;
                else if (direction == Direction.South)
                    for (int y = template.GetLength(1)/2; y < template.GetLength(1); y++)
                        template[DungeonGenerator.CellSize/2, y].Type = TileType.Floor;
            }

            MakeOpenings(template, openings);
            
            return template;
        }

        private static void MakeOpenings(TileType[,] template, Direction openings)
        {
            var size = DungeonGenerator.CellSize - 1;
            foreach (var opening in openings.ToDirectionsArray())
            {
                int x = 0, y = 0;
                
                switch (opening)
                {
                    case Direction.North:
                        x = size/2;
                        break;
                    case Direction.East:
                        x = size;
                        y = size/2;
                        break;
                    case Direction.South:
                        x = size/2;
                        y = size;
                        break;
                    case Direction.West:
                        y = size/2;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("openings");
                }

                template[x,y].Type = TileType.Floor;
            }
            
        }

        private static TileType[,] FillRoom(Direction openings)
        {
            var size = DungeonGenerator.CellSize;
            var template = new TileType[size, size];

            for (var x = 1; x < template.GetLength(0) - 1; x++)
                for (var y = 1; y < template.GetLength(1) - 1; y++)
                    template[x, y].Type = TileType.Floor;

            MakeOpenings(template, openings);

            return template;


        }
    }
}