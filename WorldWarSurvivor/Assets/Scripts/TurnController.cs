using System.Collections.Generic;
using UnityEngine;

public static class TurnController
{
    private static List<GridObject> currentMovingObjects = new();

    public static bool IsNowAnimation = currentMovingObjects.Count != 0;

    public static void AddMovingObject(GridObject animatingObject)
    {
        currentMovingObjects.Add(animatingObject);
    }

    public static void RemoveMovingObject(GridObject animatingObject)
    {
        currentMovingObjects.Remove(animatingObject);
    }

}
