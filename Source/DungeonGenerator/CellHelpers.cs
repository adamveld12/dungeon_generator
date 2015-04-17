using System;
using System.Linq;

namespace Dungeon.Generator
{
    internal static class CellHelpers
    {
        public static CellType[] CellTypes = Enum.GetValues(typeof (CellType)).Cast<CellType>().Where(x => x != CellType.None).ToArray();

        public static void Fill(this Cell cell, int x, int y, ITileMap map, GeneratorParams parameters)
        {
            TileAttributes[,] template;

            switch (cell.Type)
            {
                case CellType.Room:
                    template = FillRoom(cell.Openings);
                    break;
                case CellType.Corridor:
                    template = FillCorridor(cell.Openings);
                    break;
                default: 
                    return;
            }

            ApplyAttributes(cell, template);

            const int cs = DungeonGenerator.CellSize;

            for (var xPos = 0; xPos < template.GetLength(0); xPos++)
                for (var yPos = 0; yPos < template.GetLength(1); yPos++)
                    map[x * cs + xPos, y * cs + yPos] = template[xPos, yPos];
        }

        private static void ApplyAttributes(Cell cell, TileAttributes[,] template)
        {
            // apply doors at openings only

            // apply monster spawns in center

            // apply loot chests in corners of rooms
        }

        private static TileAttributes[,] FillCorridor(Direction openings)
        {
            var template = MakeTemplate(DungeonGenerator.CellSize);
            var openingsArray = openings.ToDirectionsArray();

            // carve the walls first, so that 
            foreach (var direction in openingsArray)
                CarveCorridors(direction, template, true);

            // we can do another pass with the floors later
            foreach (var direction in openingsArray)
                CarveCorridors(direction, template);

            MakeOpenings(template, openings);
            
            return template;
        }

        private static void CarveCorridors(Direction direction, TileAttributes[,] template, bool fillWalls = false)
        {
            // TODO dry this up
            if (direction == Direction.West)
            {
                var endX = fillWalls ? template.GetLength(0)/2 + 1 : template.GetLength(0)/2;
                for (int x = 0; x <= endX; x++)
                {
                    if (fillWalls)
                    {
                        template[x, DungeonGenerator.CellSize/2 - 1] = TileAttributes.Wall;
                        template[x, DungeonGenerator.CellSize/2 + 1] = TileAttributes.Wall;
                        template[x, DungeonGenerator.CellSize/2] = TileAttributes.Wall;
                    }
                    else
                        template[x, DungeonGenerator.CellSize/2] = TileAttributes.Floor;
                }
            }
            else if (direction == Direction.East)
            {
                var startX = fillWalls ? template.GetLength(0)/2 - 1 : template.GetLength(0)/2;
                for (int x =startX; x < template.GetLength(0); x++)
                {
                    if (fillWalls)
                    {
                        template[x, DungeonGenerator.CellSize/2 - 1] = TileAttributes.Wall;
                        template[x, DungeonGenerator.CellSize/2 + 1] = TileAttributes.Wall;
                        template[x, DungeonGenerator.CellSize/2] = TileAttributes.Wall;
                    }
                    else
                        template[x, DungeonGenerator.CellSize/2] = TileAttributes.Floor;
                }
            }
            else if (direction == Direction.North)
            {
                var endY = fillWalls ? template.GetLength(1)/2 + 1 : template.GetLength(1)/2;
                for (int y = 0; y < endY; y++)
                {
                    if (fillWalls)
                    {
                        template[DungeonGenerator.CellSize/2 - 1, y] = TileAttributes.Wall;
                        template[DungeonGenerator.CellSize/2 + 1, y] = TileAttributes.Wall;
                        template[DungeonGenerator.CellSize/2, y] = TileAttributes.Wall;
                    }
                    else
                        template[DungeonGenerator.CellSize/2, y] = TileAttributes.Floor;
                }
            }
            else if (direction == Direction.South)
            {
                var startY = fillWalls ? template.GetLength(1)/2 - 1 : template.GetLength(1)/2;
                for (int y = startY; y < template.GetLength(1); y++)
                {
                    if (fillWalls)
                    {
                        template[DungeonGenerator.CellSize/2 - 1, y] = TileAttributes.Wall;
                        template[DungeonGenerator.CellSize/2 + 1, y] = TileAttributes.Wall;
                        template[DungeonGenerator.CellSize/2, y] = TileAttributes.Wall;
                    }
                    else
                        template[DungeonGenerator.CellSize/2, y] = TileAttributes.Floor;
                }
            }
        }

        private static TileAttributes[,] MakeTemplate(int size, TileAttributes tileType = TileAttributes.Air)
        {
            var template = new TileAttributes[size, size];

            for (int x = 0; x < template.GetLength(0); x++)
                for (int y = 0; y < template.GetLength(1); y++)
                    template[x, y] = tileType;

            return template;
        }

        private static void MakeOpenings(TileAttributes[,] template, Direction openings)
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

                template[x,y] = TileAttributes.Floor;
            }
            
        }

        private static TileAttributes[,] FillRoom(Direction openings)
        {
            var size = DungeonGenerator.CellSize;
            var template = MakeTemplate(size, TileAttributes.Wall);

            for (var x = 1; x < template.GetLength(0) - 1; x++)
                for (var y = 1; y < template.GetLength(1) - 1; y++)
                    template[x, y] = TileAttributes.Floor;

            MakeOpenings(template, openings);

            return template;


        }
    }
}