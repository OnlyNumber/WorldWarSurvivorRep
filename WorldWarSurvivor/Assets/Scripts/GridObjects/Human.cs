using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : ActingObject
{
    public BoardCell MyCurrentCell;

    public float speed;

    [SerializeField] private HumanAnimator humanAnimator;

    private int MaxAmountOfEnergy = 100;

    private int _currentEnergy;

    public int CurrentEnergy
    {
        get => _currentEnergy;

        protected set
        {
            _currentEnergy = value;
        }
    }

    public HumanStats HumanStats;

    [SerializeField] protected MoveActionSO moveActionSO;

    protected MoveAction moveAction;
    protected List<AbilityAction> abilityActions = new();

    protected Dictionary<InventoryItemInfo, AbilityAction> abilityDictionary = new();

    private void Start()
    {
        moveAction = (MoveAction)moveActionSO.GetAction();
        moveAction.Initialize(this, myGrid);

        CurrentEnergy = MaxAmountOfEnergy;

        OnActivateTurn += () => CurrentEnergy = MaxAmountOfEnergy;
    }

    public override void Initialize(BoardGrid grid, BoardCell cell, GridObjectStats gridObjectStats)
    {
        base.Initialize(grid, cell, gridObjectStats);

        HumanStats = (HumanStats)gridObjectStats;

        HealthSystem.Initialize(20, 20);
        HealthSystem.OnHealthChange += DeathCheck;

        CreateActions();

        HumanStats.HumanInventoryInfo.OnEndInventoryManipulation += UpdateItemAction;

    }

    private void CreateActions()
    {
        abilityActions.Clear();

        var equipedItems = HumanStats.HumanInventoryInfo.EquipmentInfo.GetAllItemsInList();

        for (int i = 0; i < equipedItems.Count; i++)
        {
            if (equipedItems[i] == null || 
                !equipedItems[i].IsItemExist || 
                equipedItems[i].AbilityActionSO == null)
                continue;

            var action = equipedItems[i].AbilityActionSO.GetAction();
            action.Initialize(this, myGrid);
            abilityActions.Add(action);
        }
    }

    public void UpdateItemAction()
    {
        ActionWindow.Instance.ClearButtons();


        CreateActions();
        ShowWindowOfUnit();
    }

    public void ChangeEnergy(int energyChange)
    {
        Debug.Log("Change energy " + energyChange);
        CurrentEnergy += energyChange;
    }

    public override void ShowWindowOfUnit()
    {
        ActionWindow.Instance.ClearActionWindow();

        base.ShowWindowOfUnit();

        string Health = "Health " + HealthSystem.CurrentHealth.ToString() + " / " + HealthSystem.MaxHealth.ToString();
        string Energy = "Energy " + CurrentEnergy.ToString() + " / " + MaxAmountOfEnergy.ToString();

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
            (moveAction.TargetCell,moveAction.GetAccessibleCells()),
        };


        for (int i = 0; i < abilityActions.Count; i++)
        {
            int index = i;
            actions.Add((abilityActions[index].TargetCell, abilityActions[index].GetAccessibleCells()));
        }


        actionText = new()
        {
            moveActionSO.NameAction
        };

        var equipedItems = HumanStats.HumanInventoryInfo.EquipmentInfo.GetAllItemsInList();

        for (int i = 0; i < equipedItems.Count; i++)
        {
            if (!equipedItems[i].IsItemExist || equipedItems[i].AbilityActionSO == null)
                continue;
            actionText.Add(equipedItems[i].AbilityActionSO.NameAction);
        }
    }

    public List<bool> CheckActionCost()
    {

        List<bool> checkActionList = new()
        {
            moveAction.IsActionAccessible()
        };

        for (int i = 0; i < abilityActions.Count; i++)
        {
            checkActionList.Add(abilityActions[i].IsActionAccessible());
        }

        return checkActionList;
    }

    private void DeathCheck()
    {
        if (HealthSystem.CurrentHealth <= 0)
            Dispose();
    }

    public override void RemoveMyselfFromBoard()
    {
        MyCurrentCell.IsObstacle = false;
        MyCurrentCell.gridObject = null;
    }

    public override bool SetCurrentCells(BoardCell cell, bool moveToPosition = true)
    {
        if (cell == null)
            return false;

        MyCurrentCell = cell;
        MyCurrentCell.gridObject = this;
        cell.IsObstacle = true;
        if (moveToPosition)
            transform.position = cell.transform.position;

        return true;
    }

    public void SetCurrentAnimation(Animations animations)
    {
        humanAnimator.PlayAnimation(animations);
    }
}
