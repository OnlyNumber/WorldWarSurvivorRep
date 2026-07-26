using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridObject : MonoBehaviour
{
    //public BoardCell MyCurrentCell;

    protected BoardGrid myGrid;

    public HealthSystem HealthSystem = new();

    public virtual void Initialize(BoardGrid grid, BoardCell cell)
    {
        //MyCurrentCell = cell;
        myGrid = grid;

    }

    public virtual void ShowActions()
    {

    }

    public virtual void GetActions(out List<(Action<BoardCell>, HashSet<BoardCell>)> actions, out List<string> actionText)
    {
        actions = new();
        actionText = new();
    }

    public virtual void Dispose()
    {
        Destroy(gameObject);
    }

    public abstract bool SetCurrentCells(BoardCell cell, bool moveToPosition = true);

    public abstract void RemoveMyselfFromBoard();
}
