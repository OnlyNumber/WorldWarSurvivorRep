using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActingObject : GridObject
{

    //TODO: LaterMoveToFactions;
    public bool IsFriend;

    public int Initiative
    {
        get;
        private set;
    }

    public Action OnActivateTurn;

    public override void Initialize(BoardGrid grid, BoardCell cell, GridObjectStats gridObjectStats)
    {
        base.Initialize(grid, cell, gridObjectStats);

        TurnController.AddActingObject(this);
    }

    public void ActivateTurn()
    {
        OnActivateTurn?.Invoke();
    }
}
