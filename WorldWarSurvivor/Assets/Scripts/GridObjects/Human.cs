using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Human : ActingObject
{
    public BoardCell MyCurrentCell;

    //[SerializeField] private HumanAnimator humanAnimator;

    #region  Stats
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

    public HumanStats MyHumanStats;
    #endregion

    #region Actions
    [SerializeField] protected MoveActionSO moveActionSO;

    protected MoveAction moveAction;
    protected Dictionary<InventoryItemInfo, AbilityAction> abilityActions = new();

    //protected Dictionary<InventoryItemInfo, AbilityAction> lastAbilityDictionary = new();
    #endregion

    #region Model Control

    [SerializeField] private ModelController _currentModel;

    #endregion

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

        MyHumanStats = (HumanStats)gridObjectStats;

        HealthSystem.Initialize(20, 20);
        HealthSystem.OnHealthChange += DeathCheck;

        CreateActions();

        MyHumanStats.HumanInventoryInfo.OnEndInventoryManipulation += UpdateItemAction;

        EquipItems();
    }

    private void CreateActions()
    {
        abilityActions.Clear();

        var equipedItems = MyHumanStats.HumanInventoryInfo.EquipmentInfo.GetAllItemsInList();

        for (int i = 0; i < equipedItems.Count; i++)
        {
            if (equipedItems[i] == null ||
                !equipedItems[i].IsItemExist ||
                equipedItems[i].AbilityActionSO == null)
                continue;

            var action = equipedItems[i].AbilityActionSO.GetAction();
            action.Initialize(this, myGrid);
            abilityActions.Add(equipedItems[i], action);
        }
    }

    public void UpdateItemAction()
    {
        ActionWindow.Instance.ClearButtons();

        CreateActions();
        ShowWindowOfUnit();
        EquipItems();
    }

    [ContextMenu("EquipItems")]
    private void EquipItems()
    {
        _currentModel.ClearAllItems();

        var equippedItems = MyHumanStats.HumanInventoryInfo.EquipmentInfo.GetAllItemsInList();

        for (int i = 0; i < equippedItems.Count; i++)
        {
            if (equippedItems[i].IsItemExist)
                equippedItems[i].EquipItem(_currentModel);
        }
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
        var list = abilityActions.Values.ToList();

        for (int i = 0; i < abilityActions.Count; i++)
        {
            int index = i;
            actions.Add((list[index].TargetCell, list[index].GetAccessibleCells()));
        }


        actionText = new()
        {
            moveActionSO.NameAction
        };

        var equipedItems = MyHumanStats.HumanInventoryInfo.EquipmentInfo.GetAllItemsInList();

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
        var list = abilityActions.Values.ToList();

        for (int i = 0; i < abilityActions.Count; i++)
        {
            checkActionList.Add(list[i].IsActionAccessible());
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
        _currentModel.PlayAnimation(animations);
    }
}
