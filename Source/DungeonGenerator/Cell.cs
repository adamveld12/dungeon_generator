using System;
using System.Runtime.InteropServices;

namespace Dungeon.Generator
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Cell
    {
        
        [FieldOffset(0)]
        public Direction Openings;

        [FieldOffset(1)]
        public CellType Type;

        public static Cell DeadEndRoom(Direction origin)
        {
            return new Cell
            {
                Openings = origin.TurnAround()
            };
        }

        public static Cell FourWayRoom()
        {
            return new Cell
            {
                Openings = Direction.North | Direction.South | Direction.East | Direction.West
            };
        }

        public static Cell Hallway(Direction origin)
        {
            return new Cell
            {
                Openings = origin | origin.TurnAround()
            };
        }

        public static Cell ThreeWayHallway(Direction origin)
        {
            return new Cell
            {
                Openings = origin | origin.TurnLeft() | origin.TurnRight()
            };
        }
    }
}