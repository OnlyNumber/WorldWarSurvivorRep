using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TurnController
{
    //private static int _currentTurn;
    private static HashSet<GridObject> AllGridObjects = new();

    public static int CurrentTurn
    {
        get;
        private set;
    }

    //TODO: Move back to private later
    public static HashSet<ActingObject> currentMovingObjects = new();
    private static List<ActingObject> actingObjects = new();
    private static Queue<ActingObject> currentQueue = new();

    public static bool IsNowAnimation => currentMovingObjects.Count > 0;
    public static Action OnEndedAnimation;
    public static Action OnRemovingObject;

    #region MovingAnimation
    public static void AddMovingObject(ActingObject animatingObject)
    {
        currentMovingObjects.Add(animatingObject);
    }
    public static void RemoveMovingObject(ActingObject animatingObject)
    {
        currentMovingObjects.Remove(animatingObject);

        if (!IsNowAnimation)
        {
            OnEndedAnimation?.Invoke();
        }

        //BaseProgression.Instance.StartCoroutine(Removing(animatingObject));
    }

    private static System.Collections.IEnumerator Removing(ActingObject animatingObject)
    {
        yield return new WaitForSeconds(0.3f);

        currentMovingObjects.Remove(animatingObject);

        if (!IsNowAnimation)
        {
            OnEndedAnimation?.Invoke();
        }
    }
    #endregion

    #region Acting Object
    public static void AddActingObject(ActingObject animatingObject)
    {
        actingObjects.Add(animatingObject);
        currentQueue.Clear();
        SortAndCreateQueue();
    }
    public static void RemoveActingObject(ActingObject animatingObject)
    {
        actingObjects.Remove(animatingObject);
        OnRemovingObject?.Invoke();
    }
    #endregion

    public static void AddGridObject(GridObject gridObject)
    {
        AllGridObjects.Add(gridObject);
    }
    public static void RemoveGridObject(GridObject gridObject)
    {
        AllGridObjects.Remove(gridObject);
    }

    public static void ClearAllObjects()
    {
        var list = AllGridObjects.ToList();

        foreach (var item in list)
            item.Dispose();

        actingObjects.Clear();
        currentQueue.Clear();
        currentMovingObjects.Clear();
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
                Debug.Log("This happened  checkInfinity > 1000");
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

        CellSelecter.Instance.KillCurrentObjectIndicator();

        //Here will be choose who will control this object
        if (obj.IsFriend)
            CellSelecter.Instance.SetCurrentObject(obj);
        else
            EnemyController.Instance.SetCurrentObject(obj);
        //RaidersAI.Insance.SetCurrentObject

    }

    public static HashSet<ActingObject> GetFriendlyUnits()
    {
        HashSet<ActingObject> friendlyUnits = new();

        foreach (var unit in actingObjects)
        {
            if (unit.IsFriend)
                friendlyUnits.Add(unit);
        }

        return friendlyUnits;
    }

    public static HashSet<ActingObject> GetUnits(bool isFriend)
    {
        HashSet<ActingObject> units = new();

        foreach (var obj in actingObjects)
        {
            if (obj.IsFriend == isFriend)
                units.Add(obj);
        }

        return units;
    }
}
