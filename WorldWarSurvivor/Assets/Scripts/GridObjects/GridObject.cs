using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public Cell MyCurrentCell;

    protected Grid myGrid;

    public HealthSystem HealthSystem = new();

    public virtual void Initialize(Grid grid, Cell cell)
    {
        MyCurrentCell = cell;
        myGrid = grid;

    }

    public virtual void ShowActions()
    {
        
    }

    public virtual void GetActions(out List<Action<Cell>> actions, out List<string> actionText)
    {
        actions = new();
        actionText = new();
    }

    public virtual void Dispose()
    {
        Destroy(gameObject);
    }
}
