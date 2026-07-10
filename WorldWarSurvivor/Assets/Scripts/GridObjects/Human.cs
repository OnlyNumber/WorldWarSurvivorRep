using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Human : ActingObject
{
    private const float DistanceBetweenPoints = 0.1f;

    private const int WalkCost = 10;

    private const int AttackCost = 30;

    public Weapon currentWeapon;

    public float speed;

    public int maxSteps;

    [SerializeField] private HumanAnimator humanAnimator;

    private int MaxAmountOfEnergy = 100;

    private int CurrentAmountOfEnergy;

    [SerializeField] public List<InventoryItemInfo> items = new();

    [SerializeField] public EquipmentInfo equipmentInfo;


    private void Start()
    {
        CurrentAmountOfEnergy = MaxAmountOfEnergy;

        humanAnimator.AddAnimationAction(Animations.Attack, 0.9f, EndAttack);

        OnActivateTurn += () => CurrentAmountOfEnergy = MaxAmountOfEnergy;
    }




    public override void Initialize(BoardGrid grid, BoardCell cell)
    {
        base.Initialize(grid, cell);

        HealthSystem.Initialize(20, 20);
        HealthSystem.OnHealthChange += DeathCheck;
    }

    public override void ShowActions()
    {
        ActionWindow.Instance.ClearActionWindow();

        base.ShowActions();

        string Health = "Health " + HealthSystem.CurrentHealth.ToString() + " / " + HealthSystem.MaxHealth.ToString();
        string Energy = "Energy " + CurrentAmountOfEnergy.ToString() + " / " + MaxAmountOfEnergy.ToString();

        List<string> CharacteristicText = new()
        {
            Health,
            Energy
        };

        GetActions(out var actions, out var text);

        ActionWindow.Instance.CreateButtons(text, CheckActionCost());
        ActionWindow.Instance.CreateCharacteristics(CharacteristicText);
    }

    public override void GetActions(out List<(Action<BoardCell>, HashSet<BoardCell>)> actions, out List<string> actionText)
    {
        actions = new()
        {
            (Move,AccessibleCellsForMove()),
            (Attack, AccessibleCellsForWeapon())
        };

        actionText = new()
        {
            "Move",
            "Attack"
        };
    }

    private HashSet<BoardCell> AccessibleCellsForWeapon()
    {
        return currentWeapon.AccessibleCellsForAttack(myGrid, MyCurrentCell);
    }

    #region Actions

    public HashSet<BoardCell> AccessibleCellsForMove()
    {
        var cells = AStarPathfinding.FindPossiblePositions(myGrid, MyCurrentCell.Coordinate, maxSteps, true);
        cells.Remove(MyCurrentCell);

        return cells;
    }

    public void Move(BoardCell endPosition)
    {
        var path = AStarPathfinding.FindPath(myGrid, MyCurrentCell.Coordinate, endPosition.Coordinate);
        path.Remove(path[0]);

        CurrentAmountOfEnergy -= path.Count * WalkCost;

        myGrid.ChangeCellOfGridObject(MyCurrentCell, endPosition);
        StartCoroutine(MovingAnimation(path));
    }


    public void Attack(BoardCell attackingCell)
    {
        Debug.Log("Attack");

        CurrentAmountOfEnergy -= AttackCost;

        TurnController.AddMovingObject(this);
        humanAnimator.PlayAnimation(Animations.Attack);

        if (attackingCell.gridObject != null)
            currentWeapon.AttackCell(attackingCell);
    }

    private void EndAttack()
    {
        TurnController.RemoveMovingObject(this);
        humanAnimator.PlayAnimation(Animations.Idle);
        StartCoroutine(Utilities.WaitAndRun(() => humanAnimator.PlayAnimation(Animations.Idle), 0.2f));
    }

    private IEnumerator MovingAnimation(List<BoardCell> cells)
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

    #endregion

    public List<bool> CheckActionCost()
    {
        List<bool> checkActionList = new()
        {
            CurrentAmountOfEnergy > WalkCost,
            CurrentAmountOfEnergy > AttackCost
        };

        return checkActionList;
    }

    private void DeathCheck()
    {
        if (HealthSystem.CurrentHealth <= 0)
            Dispose();
    }
}
