using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RangeUnitController : UnitController
{
    private Human controllingUnit;

    private BoardGrid grid;

    private Human MyCurrentTarget;

    public override void Initialize(Human human, BoardGrid grid)
    {
        controllingUnit = human;
        this.grid = grid;
    }


    public override List<Action> CreateQueueOfActions()
    {


        FindTarget();
        List<Action> actions = new();

        actions.Add(MoveToTarget);
        actions.Add(TryAttack);

        return actions;

    }

    private void FindTarget()
    {
        var targets = TurnController.GetUnits(true);
        MyCurrentTarget = (Human)targets.First();

        var checkPos = controllingUnit.MyCurrentCell.Coordinate;

        checkPos.x++;

        if (AStarPathfinding.FindPath(grid, controllingUnit.MyCurrentCell.Coordinate, MyCurrentTarget.MyCurrentCell.Coordinate) == null)
        {
            Debug.Log("AStarPathfinding.FindPath(grid, controllingUnit.MyCurrentCell.Coordinate, MyCurrentTarget.MyCurrentCell.Coordinate) == null");
        }

        int distanceToTarget = AStarPathfinding.FindPath(grid, controllingUnit.MyCurrentCell.Coordinate, MyCurrentTarget.MyCurrentCell.Coordinate, true).Count;

        foreach (var unit in targets)
        {
            int distance = AStarPathfinding.FindPath(grid, controllingUnit.MyCurrentCell.Coordinate, (unit as Human).MyCurrentCell.Coordinate, true).Count;


            if (distanceToTarget > distance)
            {
                MyCurrentTarget = (Human)unit;
            }
        }

    }

    private void MoveToTarget()
    {
        var weapon = controllingUnit.MyHumanStats.HumanInventoryInfo.EquipmentInfo.MainHandItem;
        var targetCoordinate = MyCurrentTarget.MyCurrentCell.Coordinate;

        FogOfWar.FindVisibleCellsFromPosition(grid, targetCoordinate, out var positionsForAttack, (controllingUnit.abilityActions[weapon] as GunShootAction).GetAttackRange());

        float minDistance = Vector3.Distance(positionsForAttack.First().transform.position, controllingUnit.MyCurrentCell.transform.position); ;
        BoardCell attackPosition = null;

        foreach (var item in positionsForAttack)
        {
            float distanceBetweenCells = Vector3.Distance(item.transform.position, controllingUnit.MyCurrentCell.transform.position);

            if (distanceBetweenCells < minDistance && !item.IsObstacle)
            {
                attackPosition = item;
                minDistance = distanceBetweenCells;
            }
        }

        var path = AStarPathfinding.FindPath(grid, controllingUnit.MyCurrentCell.Coordinate, attackPosition.Coordinate);

        var accessiblePosition = controllingUnit.GetMoveAccessibleCells();
        path.Reverse();

        foreach (var item in path)
        {
            if (accessiblePosition.Contains(item))
            {
                attackPosition = item;
                break;
            }

        }


        controllingUnit.SetTargetMove(attackPosition);
    }

    private void TryAttack()
    {
        var weapon = controllingUnit.MyHumanStats.HumanInventoryInfo.EquipmentInfo.MainHandItem;

        var weaponAction = controllingUnit.abilityActions[weapon];

        while (weaponAction.IsActionAccessible())
        {
            if (weaponAction.GetAccessibleCells().Contains(MyCurrentTarget.MyCurrentCell))
                weaponAction.TargetCell(MyCurrentTarget.MyCurrentCell);
            else
                break;
        }
    }
}
