using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProgressionData
{
    public List<HumanStats> Roster = new();

    public List<HumanStats> CurrentCommand = new();

    public InventoryInfo PlayerInventory;

    public Vector2 InventorySize;
}
