using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActingObject : GridObject
{
    public int Initiative
    {
        get;
        private set;
    }

    public override void Initialize(Grid grid, Cell cell)
    {
        base.Initialize(grid, cell);

        TurnController.AddActingObject(this);
    }
}
