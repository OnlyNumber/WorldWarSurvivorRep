using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class TeamDefiner 
{
    public static List<ActingObject> allObjects = new();

    public static void CreateObject(BoardGrid grid, Vector2Int position, GridObject prefab)
    {
        var obj = grid.SpawnGridObject(position, prefab);

        allObjects.Add(obj as ActingObject);
    }
}
