using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    None = 0,
    North = 1,
    NorthEast = 2,
    East = 3,
    SouthEast = 4,
    South = 5,
    SouthWest = 6,
    West = 7,
    NorthWest = 8
}

public static class UtilityMethods
{
    public static Direction InvertDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:     return Direction.South;
            case Direction.NorthEast: return Direction.SouthWest;
            case Direction.East:      return Direction.West;
            case Direction.SouthEast: return Direction.NorthWest;
            case Direction.South:     return Direction.North;
            case Direction.SouthWest: return Direction.NorthEast;
            case Direction.West:      return Direction.East;
            case Direction.NorthWest: return Direction.SouthEast;

            default: return Direction.None;
        }
    }

    public static Vector2Int DirectionToVector2Int(Direction direction)
    {
        switch (direction)
        {
            case Direction.None:      return new Vector2Int(0, 0);
            case Direction.North:     return new Vector2Int(0, 1);
            case Direction.NorthEast: return new Vector2Int(1, 1);
            case Direction.East:      return new Vector2Int(1, 0);
            case Direction.SouthEast: return new Vector2Int(1, -1);
            case Direction.South:     return new Vector2Int(0, -1);
            case Direction.SouthWest: return new Vector2Int(-1, -1);
            case Direction.West:      return new Vector2Int(-1, 0);
            case Direction.NorthWest: return new Vector2Int(-1, 1);

            default: return Vector2Int.zero;
        }
    }
}
