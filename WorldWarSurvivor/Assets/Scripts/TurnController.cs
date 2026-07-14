using System;
using System.Collections.Generic;
using UnityEngine;

public static class TurnController
{
    //private static int _currentTurn;

    public static int CurrentTurn
    {
        get;
        private set;
    }

    private static List<ActingObject> currentMovingObjects = new();

    private static List<ActingObject> actingObjects = new();

    private static Queue<ActingObject> currentQueue = new();

    public static bool IsNowAnimation => currentMovingObjects.Count > 0;

    public static Action OnEndedAnimation;

    public static void AddMovingObject(ActingObject animatingObject)
    {
        currentMovingObjects.Add(animatingObject);
    }

    public static void RemoveMovingObject(ActingObject animatingObject)
    {
        currentMovingObjects.Remove(animatingObject);

        if(!IsNowAnimation)
        {
            OnEndedAnimation?.Invoke();
        }
    }

    public static void AddActingObject(ActingObject animatingObject)
    {
        actingObjects.Add(animatingObject);
        currentQueue.Clear();
        SortAndCreateQueue();
    }

    public static void RemoveActingObject(ActingObject animatingObject)
    {
        actingObjects.Remove(animatingObject);
    }

    public static void SortAndCreateQueue()
    {
        ActingObject tempObject;

        bool isChanged = false;

        int checkInfinity = 0;

        do
        {

            checkInfinity++;
            if (checkInfinity > 1000)
            {
                Debug.Log("This happened ");
                break;
            }
            isChanged = false;

            for (int i = 0; i < actingObjects.Count - 1; i++)
            {
                if (actingObjects[i].Initiative < actingObjects[i + 1].Initiative)
                {
                    tempObject = actingObjects[i];
                    actingObjects[i] = actingObjects[i + 1];
                    actingObjects[i + 1] = tempObject;
                    isChanged = true;
                    break;
                }
            }


        } while (isChanged);

        foreach (var item in actingObjects)
        {
            currentQueue.Enqueue(item);
        }

    }

    public static ActingObject GetNextActingObject()
    {
        if (currentQueue.Count == 0)
            SortAndCreateQueue();

        return currentQueue.Dequeue();
    }

    public static void SetNextTurn()
    {

        CellSelecter.Instance.ClearCurrentCells();
        ActionWindow.Instance.ClearActionWindow();

        var obj = GetNextActingObject();

        obj.ActivateTurn();

        CellSelecter.Instance.NoCurrentObject();

        //Here will be choose who will control this object
        if (obj.IsFriend)
            CellSelecter.Instance.SetCurrentObject(obj);
        else
            EnemyController.Instance.SetCurrentObject(obj);
        //RaidersAI.Insance.SetCurrentObject

    }
}
