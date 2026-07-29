using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HumanInventoryInfo : InventoryInfo
{
    [SerializeField] public EquipmentInfo EquipmentInfo = new();

    public override void GetItemsSO()
    {
        base.GetItemsSO();

        foreach (var item in EquipmentInfo.GetAllItemsInList())
        {
            item.GetMyItemFromIndex();
        }
    }

    public override void SetItemsSO()
    {
        base.SetItemsSO();

        var list = EquipmentInfo.GetAllItemsInList();

        if (list == null || list.Count == 0)
            return;

        foreach (var item in list)
        {
            if (item != null && item.IsItemExist)
                item.SetMyItemIndex();
        }
    }
}
