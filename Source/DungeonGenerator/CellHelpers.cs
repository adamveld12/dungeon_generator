using System;
using System.Linq;

namespace Dungeon.Generator
{
    internal static class CellHelpers
    {
        public static CellType[] CellTypes = Enum.GetValues(typeof (CellType)).Cast<CellType>().Where(x => x != CellType.None).ToArray();

        public static void Fill(this Cell cell, int x, int y, ITileMap map, GeneratorParams parameters)
        {
            Tile[,] template;

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

            const int cs = CellBasedGenerator.CellSize;

            Iterate(template, ( loc, tile) => map[x*cs + loc.X, y*cs + loc.Y] = tile);
        }

        private static void ApplyAttributes(Cell cell, Tile[,] template)
        {
            int w = template.GetLength(0), h = template.GetLength(1);
            var attr = cell.Attributes;

            if ((attr & AttributeType.Exit) == AttributeType.Exit)
                template[w/2, h/2].Attributes = AttributeType.Exit;
            else if ((attr & AttributeType.Entry) == AttributeType.Entry)
                template[w/2, h/2].Attributes = AttributeType.Entry;

            // apply monster spawns in center
            if ((attr & AttributeType.MobSpawn) == AttributeType.MobSpawn)
            {
                Iterate(template, (point, tile) => 
                {
                   if (tile.MaterialType == MaterialType.Floor && (point.X > w/2.0f && point.Y < h/2.0f)) 
                   {
                        template[point.X, point.Y].Attributes |= AttributeType.MobSpawn;
                       return false;
                   }
                    return true;
                });
            }

            // apply loot chests in corners of rooms
            if ((attr & AttributeType.Loot) == AttributeType.Loot )
            {
                if (cell.Type == CellType.Room)
                {
                    template[1, 1].Attributes |= AttributeType.Loot;
                    template[w - 2, h - 2].Attributes |= AttributeType.Loot;
                }
                else if (cell.Type == CellType.Corridor)
                {
                    template[w/2, h/2].Attributes |= AttributeType.Loot;
                }
            }
        }

        private static Tile[,] FillCorridor(Direction openings)
        {
            var template = MakeTemplate(CellBasedGenerator.CellSize);
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

        private static void CarveCorridors(Direction direction, Tile[,] template, bool fillWalls = false)
        {
            if (direction == Direction.West)
            {
                var endX = fillWalls ? template.GetLength(0)/2 + 1 : template.GetLength(0)/2;
                for (int x = 0; x <= endX; x++)
                {
                    if (fillWalls)
                    {
                        template[x, CellBasedGenerator.CellSize/2 - 1] = Tile.Wall;
                        template[x, CellBasedGenerator.CellSize/2 + 1] = Tile.Wall;
                        template[x, CellBasedGenerator.CellSize/2] = Tile.Wall;
                    }
                    else
                        template[x, CellBasedGenerator.CellSize/2] = Tile.Floor;
                }
            }
            else if (direction == Direction.East)
            {
                var startX = fillWalls ? template.GetLength(0)/2 - 1 : template.GetLength(0)/2;
                for (int x =startX; x < template.GetLength(0); x++)
                {
                    if (fillWalls)
                    {
                        template[x, CellBasedGenerator.CellSize/2 - 1] = Tile.Wall;
                        template[x, CellBasedGenerator.CellSize/2 + 1] = Tile.Wall;
                        template[x, CellBasedGenerator.CellSize/2] = Tile.Wall;
                    }
                    else
                        template[x, CellBasedGenerator.CellSize/2] = Tile.Floor;
                }
            }
            else if (direction == Direction.North)
            {
                var endY = fillWalls ? template.GetLength(1)/2 + 1 : template.GetLength(1)/2;
                for (int y = 0; y < endY; y++)
                {
                    if (fillWalls)
                    {
                        template[CellBasedGenerator.CellSize/2 - 1, y] = Tile.Wall;
                        template[CellBasedGenerator.CellSize/2 + 1, y] = Tile.Wall;
                        template[CellBasedGenerator.CellSize/2, y] = Tile.Wall;
                    }
                    else
                        template[CellBasedGenerator.CellSize/2, y] = Tile.Floor;
                }
            }
            else if (direction == Direction.South)
            {
                var startY = fillWalls ? template.GetLength(1)/2 - 1 : template.GetLength(1)/2;
                for (int y = startY; y < template.GetLength(1); y++)
                {
                    if (fillWalls)
                    {
                        template[CellBasedGenerator.CellSize/2 - 1, y] = Tile.Wall;
                        template[CellBasedGenerator.CellSize/2 + 1, y] = Tile.Wall;
                        template[CellBasedGenerator.CellSize/2, y] = Tile.Wall;
                    }
                    else
                        template[CellBasedGenerator.CellSize/2, y] = Tile.Floor;
                }
            }
        }

        private static Tile[,] MakeTemplate(int size, Tile tileType = default(Tile))
        {
            var template = new Tile[size, size];

            for (var x = 0; x < template.GetLength(0); x++)
                for (var y = 0; y < template.GetLength(1); y++)
                    template[x, y] = tileType;

            return template;
        }

        private static void MakeOpenings(Tile[,] template, Direction openings)
        {
            const int size = CellBasedGenerator.CellSize - 1;
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

                template[x,y] = Tile.Floor;
            }
        }

        private static Tile[,] FillRoom(Direction openings)
        {
            const int size = CellBasedGenerator.CellSize;
            var template = MakeTemplate(size, Tile.Wall);

            for (var x = 1; x < template.GetLength(0) - 1; x++)
                for (var y = 1; y < template.GetLength(1) - 1; y++)
                    template[x, y] = Tile.Floor;

            MakeOpenings(template, openings);

            return template;


        }



        private static void Iterate(Tile[,] template, Func<Point, Tile, bool> callback)
        {
            for (var xPos = 0; xPos < template.GetLength(0); xPos++)
                for (var yPos = 0; yPos < template.GetLength(1); yPos++)
                    if (!callback(new Point {X = xPos, Y = yPos}, template[xPos, yPos]))
                        return;
        }

        private static void Iterate(Tile[,] template, Func<Point, Tile, Tile> callback)
        {
            for (var xPos = 0; xPos < template.GetLength(0); xPos++)
                for (var yPos = 0; yPos < template.GetLength(1); yPos++)
                    template[xPos, yPos] = callback(new Point{X = xPos, Y = yPos}, template[xPos, yPos]);
        }

        private static void Iterate(Tile[,] template, Action<Point, Tile> callback)
        {
            for (var xPos = 0; xPos < template.GetLength(0); xPos++)
                for (var yPos = 0; yPos < template.GetLength(1); yPos++)
                    callback(new Point{X = xPos, Y = yPos}, template[xPos, yPos]);
        }
    }
}