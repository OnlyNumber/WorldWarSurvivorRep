using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : GridObject
{
    private const float DistanceBetweenPoints = 0.1f;

    public Weapon currentWeapon;

    public float speed;

    public int maxSteps;

    public override void Initialize(Grid grid, Cell cell)
    {
        base.Initialize(grid, cell);

        HealthSystem.Initialize(20, 20);
        HealthSystem.OnHealthChange += DeathCheck;
    }

    public override void ShowActions()
    {
        base.ShowActions();

        Debug.Log("Show action of human");

        string Health = "Health " + HealthSystem.CurrentHealth.ToString();

        List<string> CharacteristicText = new()
        {
            Health
        };

        ActionWindow.Instance.CreateCharacteristics(CharacteristicText);
    }

    public override void GetActions(out List<(Action<Cell>, HashSet<Cell>)> actions, out List<string> actionText)
    {
        actions = new()
        {
            (Move,AccessibleCellsForMove()),
            (Attack,AccessibleCellsForAttack())
        };

        actionText = new()
        {
            "Move",
            "Attack"
        };
    }

    public HashSet<Cell> AccessibleCellsForMove()
    {
        return AStarPathfinding.FindPossiblePositions(myGrid, MyCurrentCell.Coordinate, maxSteps);
    }

    public void Move(Cell endPosition)
    {
        var path = AStarPathfinding.FindPath(myGrid, MyCurrentCell.Coordinate, endPosition.Coordinate);
        myGrid.ChangeCellOfGridObject(MyCurrentCell, endPosition);
        StartCoroutine(MovingAnimation(path));
    }

    // In future move this functional to the weapon
    public HashSet<Cell> AccessibleCellsForAttack()
    {
        HashSet<Cell> targets = new();

        foreach (var item in AStarPathfinding.FindPossiblePositions(myGrid, MyCurrentCell.Coordinate, maxSteps))
        {
            if (item.gridObject is Human)
                targets.Add(item);
        }
        
        return targets;
    }

    public void Attack(Cell attackingCell)
    {
        if (attackingCell.gridObject != null)
            attackingCell.gridObject.HealthSystem.ChangeHealth(-currentWeapon.Damage);
    }

    private IEnumerator MovingAnimation(List<Cell> cells)
    {
        int index = 0;
        do
        {
            var cellPosition = cells[index].transform.position;

            transform.position = Vector3.MoveTowards(transform.position, cellPosition, Time.deltaTime * speed);

            if (Vector3.Distance(transform.position, cellPosition) < DistanceBetweenPoints)
                index++;

            yield return null;

        } while (index < cells.Count);

    }

    private void DeathCheck()
    {
        if (HealthSystem.CurrentHealth <= 0)
            Dispose();
    }
}
