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
    public MissionData CurrentMission;
    public string MissionBackgroundMap;

    public bool IsMissionCompleted;

    public int Money;

    public BaseProgressionData()
    {
        PlayerInventory.Size.x = 10;
        PlayerInventory.Size.y = 20;
    }

    public void GetMyItemFromIndex()
    {
        foreach (var item in Roster)
            item.HumanInventoryInfo.GetItemsSO();
        
        foreach (var item in CurrentCommand)
            item.HumanInventoryInfo.GetItemsSO();

        PlayerInventory.GetItemsSO();
    }
    
    public void SetMyItemIndex()
    {
        foreach (var item in Roster)
            item.HumanInventoryInfo.SetItemsSO();
        
        foreach (var item in CurrentCommand)
            item.HumanInventoryInfo.SetItemsSO();

        PlayerInventory.SetItemsSO();
    }

}
