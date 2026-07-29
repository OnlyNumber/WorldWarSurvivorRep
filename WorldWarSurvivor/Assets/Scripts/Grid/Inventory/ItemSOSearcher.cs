using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemSOSearcher
{
    private const string PathToItems = "ScriptableObjects/Items";

    private static InventoryItemSO[] inventoryItemSOs;

    public static InventoryItemSO GetItemFromIndex(int uniqueIndex)
    {
        if (inventoryItemSOs == null || inventoryItemSOs.Length == 0)
            LoadAllItems();

        foreach (var item in inventoryItemSOs)
        {
            if (item.UniqueIndex == uniqueIndex)
                return item;
        }

        return null;
    }

    private static void LoadAllItems()
    {
        inventoryItemSOs = Resources.LoadAll<InventoryItemSO>(PathToItems);
    }

}
