using System;
using System.Collections;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static IEnumerator WaitAndRun(Action action, float time = 0.1f)
    {
        yield return new WaitForSeconds(time);

        action.Invoke();
    }

    public static Vector3Int DirectionToPosition(Direction dir)
    {
        switch (dir)
        {
            case Direction.Right: return new Vector3Int(1, 0, -1);
            case Direction.Down: return new Vector3Int(-1, 0, -1);
            case Direction.Left: return new Vector3Int(-1, 0, 1);
            default: return new Vector3Int(1, 0, 1); // Up
        }
    }

    public static Quaternion DirectionToRotation(Direction dir)
    {
        switch (dir)
        {
            case Direction.Right: return Quaternion.Euler(0f, 90f, 0f);
            case Direction.Down:  return Quaternion.Euler(0f, 180f, 0f);
            case Direction.Left:  return Quaternion.Euler(0f, 270f, 0f);
            default:              return Quaternion.Euler(0f, 0f, 0f); // Up
        }
    }

    public static Vector2Int SMTHAboutDirection(Direction dir, Vector2Int coordinate)
    {
        switch (dir)
        {
            case Direction.Right: return new Vector2Int(coordinate.y, -coordinate.x );
            case Direction.Down:  return new Vector2Int(-coordinate.x, -coordinate.y );
            case Direction.Left:  return new Vector2Int(-coordinate.y, coordinate.x );
            default:              return coordinate; // Up
        }
    }
}
