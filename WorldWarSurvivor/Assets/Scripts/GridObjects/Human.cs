using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : ActingObject
{
    private const float DistanceBetweenPoints = 0.1f;

    public Weapon currentWeapon;

    public float speed;

    public int maxSteps;

    [SerializeField] private HumanAnimator humanAnimator;

    private float MaxAmountOfEnergy;

    private float CurrentAmountOfEnergy;

    private void Start()
    {
        humanAnimator.AddAnimationAction(Animations.Attack, 0.9f, EndAttack);
    }

    private void EndAttack()
    {
        TurnController.RemoveMovingObject(this);
        humanAnimator.PlayAnimation(Animations.Idle);
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        humanAnimator.PlayAnimation(Animations.Idle);
    }

    public override void Initialize(Grid grid, Cell cell)
    {
        base.Initialize(grid, cell);

        HealthSystem.Initialize(20, 20);
        HealthSystem.OnHealthChange += DeathCheck;
    }

    public override void ShowActions()
    {
        base.ShowActions();

        string Health = "Health " + HealthSystem.CurrentHealth.ToString() + " / " + HealthSystem.MaxHealth.ToString();
        string Energy = "Energy " + CurrentAmountOfEnergy.ToString() + " / " + MaxAmountOfEnergy.ToString();

        List<string> CharacteristicText = new()
        {
            Health,
            Energy
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
        var cells = AStarPathfinding.FindPossiblePositions(myGrid, MyCurrentCell.Coordinate, maxSteps, true);
        cells.Remove(MyCurrentCell);

        return cells;
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
        Debug.Log("AccessibleCellsForAttack");

        foreach (var item in AStarPathfinding.FindPossiblePositions(myGrid, MyCurrentCell.Coordinate, maxSteps, false))
        {
            if (item.gridObject is Human)
            {
                targets.Add(item);
            }
        }

        targets.Remove(MyCurrentCell);

        return targets;
    }

    public void Attack(Cell attackingCell)
    {

        TurnController.AddMovingObject(this);
        humanAnimator.PlayAnimation(Animations.Attack);

        if (attackingCell.gridObject != null)
            attackingCell.gridObject.HealthSystem.ChangeHealth(-currentWeapon.Damage);
    }

    private IEnumerator MovingAnimation(List<Cell> cells)
    {
        int index = 0;

        TurnController.AddMovingObject(this);

        humanAnimator.PlayAnimation(Animations.Walk);

        do
        {
            var cellPosition = cells[index].transform.position;

            transform.position = Vector3.MoveTowards(transform.position, cellPosition, Time.deltaTime * speed);

            Vector3 direction = (cellPosition - transform.position).normalized;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);


            if (Vector3.Distance(transform.position, cellPosition) < DistanceBetweenPoints)
                index++;

            yield return null;

        } while (index < cells.Count);

        humanAnimator.PlayAnimation(Animations.Idle);
        TurnController.RemoveMovingObject(this);

    }

    private void DeathCheck()
    {
        if (HealthSystem.CurrentHealth <= 0)
            Dispose();
    }
}
