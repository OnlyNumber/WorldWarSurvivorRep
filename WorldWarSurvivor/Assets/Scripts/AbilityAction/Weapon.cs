using System.Collections.Generic;
using UnityEngine;

public class Weapon : AbilityAction
{
    public int Damage;

    public int AttackRange;

    public int AttackEnergyCost;

    private RuntimeAnimatorController WeaponAnimator;

    private Human GetHuman()
    {
        return CurrentActingObject as Human;
    }

    public override void Initialize(ActingObject actingObject, BoardGrid myGrid)
    {
        CurrentActingObject = actingObject;
        this.myGrid = myGrid;

        //Create here model and place it
    }

    public override void Dispose()
    {
        //remove model

        throw new System.NotImplementedException();
    }

    public override HashSet<BoardCell> GetAccessibleCells(BoardGrid boardGrid, BoardCell boardCell)
    {
        HashSet<BoardCell> targets = new();

        foreach (var item in AStarPathfinding.GetReachableTiles(boardCell.Coordinate, AttackRange, boardGrid, false))
        {
            if (boardGrid.GetCell(item).gridObject is Human)
            {
                targets.Add(boardGrid.GetCell(item));
            }
        }

        targets.Remove(boardCell);

        return targets;
    }

    public override void TargetCell(BoardCell attackingCell)
    {
        if (attackingCell.gridObject != null)
            attackingCell.gridObject.HealthSystem.ChangeHealth(-Damage);
    }

    public override bool IsActionAccessible()
    {

        return GetHuman().CurrentEnergy > AttackEnergyCost;
    }

}