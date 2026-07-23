using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseProgressionData
{
    public List<HumanStats> Roster = new();

    public List<HumanStats> CurrentCommand = new();

    public InventoryInfo PlayerInventory = new();

    public int MapIndex = 0;

    public BaseProgressionData()
    {
        PlayerInventory.Size.x = 10;
        PlayerInventory.Size.y = 20;


    }
}
