using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : AbilityAction
{
    public int Heal = 10;
    private Human CurrentHuman;

    public HealAction(ActionSO actionSO) : base(actionSO)
    {
        Heal = (actionSO as HealActionSO).HealingHealth;
    }

    public override void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public override HashSet<BoardCell> GetAccessibleCells()
    {
        HashSet<BoardCell> targets = new();
        targets.Add(CurrentHuman.MyCurrentCell);

        return targets;
    }

    public override void Initialize(ActingObject actingObject, BoardGrid myGrid)
    {
        CurrentActingObject = actingObject;
        this.myGrid = myGrid;

        CurrentHuman = CurrentActingObject as Human;
    }

    public override bool IsActionAccessible()
    {
        return true;
    }

    public override void TargetCell(BoardCell attackingCell)
    {
        if (attackingCell.gridObject != null)
            attackingCell.gridObject.HealthSystem.ChangeHealth(+Heal);
    }
}
