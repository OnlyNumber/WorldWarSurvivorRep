using System.Collections.Generic;
using UnityEngine;

public class GunShootAction : AbilityAction
{
    public int Damage;

    public int AttackRange;

    public int AttackEnergyCost;

    private RuntimeAnimatorController WeaponAnimator;

    private Human CurrentHuman;

    public override void Initialize(ActingObject actingObject, BoardGrid myGrid)
    {
        CurrentActingObject = actingObject;
        this.myGrid = myGrid;

        CurrentHuman = CurrentActingObject as Human;

        //Create here model and place it
    }

    public override void Dispose()
    {
        //remove model

        throw new System.NotImplementedException();
    }

    public override HashSet<BoardCell> GetAccessibleCells()
    {
        HashSet<BoardCell> targets = new();

        foreach (var item in AStarPathfinding.GetReachableTiles(CurrentHuman.MyCurrentCell.Coordinate, AttackRange, myGrid, false))
        {
            if (myGrid.GetCell(item).gridObject is Human)
            {
                targets.Add(myGrid.GetCell(item));
            }
        }

        targets.Remove(CurrentHuman.MyCurrentCell);

        return targets;
    }

    public override void TargetCell(BoardCell attackingCell)
    {
            CurrentHuman.ChangeEnergy(AttackEnergyCost);

            TurnController.AddMovingObject(CurrentHuman);
            CurrentHuman.SetCurrentAnimation(Animations.Attack);

        if (attackingCell.gridObject != null)
            attackingCell.gridObject.HealthSystem.ChangeHealth(-Damage);

            EndAttack();
    }

    public override bool IsActionAccessible()
    {
        return CurrentHuman.CurrentEnergy > AttackEnergyCost;
    }

    private void EndAttack()
    {
        TurnController.RemoveMovingObject(CurrentHuman);
        CurrentHuman.SetCurrentAnimation(Animations.Idle);
        CurrentHuman.StartCoroutine(Utilities.WaitAndRun(() => CurrentHuman.SetCurrentAnimation(Animations.Idle), 0.2f));

        CurrentHuman.SetCurrentAnimation(Animations.Idle);
    }
}