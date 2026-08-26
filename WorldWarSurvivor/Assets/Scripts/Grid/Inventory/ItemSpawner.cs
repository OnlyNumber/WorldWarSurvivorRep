using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<InventoryItemSO> inventoryItemInfos;

    public int ItemIndex;

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        var item = new InventoryItemInfo(inventoryItemInfos[ItemIndex]);
        InventorySystem.Instance.SpawnItem(item);
    }
}
