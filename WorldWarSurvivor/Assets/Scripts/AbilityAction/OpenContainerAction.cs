using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenContainerAction : AbilityAction
{
    private int _openContainerEnergyCost = 5;


    private Human CurrentHuman;


    public OpenContainerAction(ActionSO actionSO) : base(actionSO)
    {
    }

    public override void Initialize(ActingObject actingObject, BoardGrid myGrid)
    {
        CurrentActingObject = actingObject;
        this.myGrid = myGrid;

        CurrentHuman = CurrentActingObject as Human;
    }

    public override HashSet<BoardCell> GetAccessibleCells(Vector2Int CellPosition)
    {
        HashSet<BoardCell> cells = new();

        for (int x = -1; x < 1; x += 2)
        {
            var cell = myGrid.GetCell(CellPosition.x + x, CellPosition.y);
            if (cell != null && cell.gridObject != null && cell.gridObject is Container)
                cells.Add(cell);
        }

        for (int y = -1; y < 1; y += 2)
        {
            var cell = myGrid.GetCell(CellPosition.x, CellPosition.y + y);
            if (cell != null && cell.gridObject != null && cell.gridObject is Container)
                cells.Add(cell);
        }

        return cells;
    }


    public override HashSet<BoardCell> GetAccessibleCells()
    {
        return GetAccessibleCells(CurrentHuman.MyCurrentCell.Coordinate);
    }

    public override bool IsActionAccessible()
    {
        return CurrentHuman.CurrentEnergy > _openContainerEnergyCost;
    }

    public override void TargetCell(BoardCell targetContainer)
    {
        var container = targetContainer.gridObject as Container;

        InventoryWindow.Instance.OpenWindow(CurrentHuman.MyHumanStats.HumanInventoryInfo, container.GetContainer());
    }

    public override void Dispose()
    {

    }
}
