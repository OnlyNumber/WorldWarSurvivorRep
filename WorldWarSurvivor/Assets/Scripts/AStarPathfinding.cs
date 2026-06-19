using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public static class AStarPathfinding
{


    public static List<Cell> GetPath(Grid searchingGrid, Vector2Int startPoint, Vector2Int endPoint)
    {

        return null;
    }
}

public struct PathCell
{
    public Vector2Int CellCoordinates;

    public float CellCost;

    public float FullCost;
}