using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeUnitController : UnitController
{
    private Human controllingUnit;

    private BoardGrid boardGrid;

    private ActingObject MyTarget;

    //Change this later to getting from controlling unit
    private Weapon weapon;

    //Change later to the weapon;
    private int AttackRange => 1;

    public void Initialize(Human human, BoardGrid grid)
    {
        controllingUnit = human;
        boardGrid = grid;
        weapon = controllingUnit.currentWeapon;
        SetTargets(TeamDefiner.allObjects);
    }

    private void SetTargets(List<ActingObject> actingObjects)
    {
        foreach (var item in actingObjects)
        {
            if (item.IsFriend)
                MyTarget = item;
        }
    }

    public List<Action> CreateQueueOfActions()
    {
        List<Action> actions = new();

        actions.Add(MoveToTarget);
        actions.Add(TryAttack);

        return actions;

    }

    private void MoveToTarget()
    {
        var path = AStarPathfinding.FindPath(boardGrid, controllingUnit.MyCurrentCell.Coordinate, Vector2Int.zero, true);

        path.Remove(path[0]);
        path.Remove(path[path.Count - 1]);

        int distanceToTarget = path.Count;
        distanceToTarget -= (AttackRange - 1);

        controllingUnit.Move(path[distanceToTarget - 1]);
    }

    private void TryAttack()
    {
        /*if (weapon.AccessibleCellsForAttack(boardGrid, controllingUnit.MyCurrentCell).Contains(MyTarget.MyCurrentCell))
        {
            controllingUnit.Attack(MyTarget.MyCurrentCell);
        }*/
    }
}
