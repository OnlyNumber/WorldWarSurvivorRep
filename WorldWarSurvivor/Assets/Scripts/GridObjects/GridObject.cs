using System;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public BoardCell MyCurrentCell;

    protected BoardGrid myGrid;

    public HealthSystem HealthSystem = new();

    [field: SerializeField]
    public bool IsObstacle
    {
        private set;
        get;
    }

    public virtual void Initialize(BoardGrid grid, BoardCell cell)
    {
        MyCurrentCell = cell;
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
}
