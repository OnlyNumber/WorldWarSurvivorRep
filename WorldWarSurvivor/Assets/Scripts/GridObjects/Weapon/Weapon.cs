using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Damage;

    public int AttackRange;

    public int AttackEnergyCost;

    private RuntimeAnimatorController WeaponAnimator;

    public HashSet<BoardCell> AccessibleCellsForAttack(BoardGrid boardGrid, BoardCell boardCell)
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

    public void AttackCell(BoardCell attackingCell)
    {
        if (attackingCell.gridObject != null)
            attackingCell.gridObject.HealthSystem.ChangeHealth(-Damage);
    }
}
