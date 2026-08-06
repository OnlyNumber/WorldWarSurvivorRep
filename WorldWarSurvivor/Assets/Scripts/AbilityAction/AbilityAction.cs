using System;
using System.Collections;
using System.Collections.Generic;

public abstract class AbilityAction : IDisposable
{   
    protected ActingObject CurrentActingObject;

    protected BoardGrid myGrid;
    
    public AbilityAction(ActionSO actionSO)
    {
        
    }
    
    public abstract void Initialize(ActingObject actingObject, BoardGrid myGrid);

    public abstract void Dispose();

    public abstract HashSet<BoardCell> GetAccessibleCells();

    public abstract void TargetCell(BoardCell attackingCell);

    public abstract bool IsActionAccessible();
}