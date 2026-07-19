using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<InventoryItemInfo> inventoryItemInfos;

    public int ItemIndex;

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        InventorySystem.Instance.SpawnItem(inventoryItemInfos[ItemIndex]);
    }
}
