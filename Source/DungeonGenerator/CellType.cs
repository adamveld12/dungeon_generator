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

            }

            return template;
        }

        private static TileType[,] FillCorridor(Direction openings)
        {
            var template = new TileType[16,16];

            
            return template;
        }

        private static void MakeOpenings(TileType[,] template, Direction openings)
        {
            foreach (var opening in openings.ToDirectionsArray())
            {
                int x = 0, y = 0;
                
                switch (opening)
                {
                    case Direction.North:
                        x = 7;
                        break;
                    case Direction.East:
                        x = 15;
                        y = 7;
                        break;
                    case Direction.South:
                        x = 7;
                        y = 15;
                        break;
                    case Direction.West:
                        y = 7;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("openings");
                }

                template[x,y].Type = TileType.Floor;
            }
            
        }

        private static TileType[,] FillRoom(Direction openings)
        {
            var template = new TileType[16,16];

            for (var x = 1; x < template.GetLength(0) - 1; x++)
                for (var y = 1; y < template.GetLength(1) - 1; y++)
                    template[x, y].Type = TileType.Floor;

            MakeOpenings(template, openings);

            return template;


        }
    }
}