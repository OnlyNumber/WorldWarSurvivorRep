using System;
using System.Collections;
using System.Collections.Generic;

public abstract class AbilityAction : IDisposable
{   
    protected ActingObject CurrentActingObject;

    protected BoardGrid myGrid;

    
    public abstract void Initialize(ActingObject actingObjectm, BoardGrid myGrid);

    public abstract void Dispose();

    public abstract HashSet<BoardCell> GetAccessibleCells(BoardGrid boardGrid, BoardCell boardCell);

    public abstract void TargetCell(BoardCell attackingCell);

    public abstract bool IsActionAccessible();
}