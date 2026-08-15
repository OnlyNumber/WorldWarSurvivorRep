using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Container : GridObject
{
    public Vector2Int Size;
    
    private InventoryInfo containerInventory = new();

    public GameObject containerModel;

    public BoardCell CurrentCell; 

    private void Awake() 
    {
        containerInventory.Size = Size;
    }

    public InventoryInfo GetContainer()
    {
        return containerInventory;
    }

    public override void RemoveMyselfFromBoard()
    {
        CurrentCell.IsObstacle = false;
        CurrentCell.gridObject = null;
    }

    public override bool SetCurrentCells(BoardCell cell, bool moveToPosition = true)
    {
        if (cell == null)
            return false;

        CurrentCell = cell;
        CurrentCell.gridObject = this;
        cell.IsObstacle = true;
        if (moveToPosition)
            transform.position = cell.transform.position;

        return true;
    }

    public override void Hide()
    {
        containerModel.SetActive(false);
    }

    public override void Show()
    {
        containerModel.SetActive(true);
    }
}