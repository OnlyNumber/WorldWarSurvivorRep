using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public BoardGrid boardGrid;
    public Container firstChest;
    public Vector2Int spawnPosition;

    [SerializeField] private List<InventoryItemSO> items;

    [ContextMenu("Check")]
    public void CreateContainer()
    {
        var container = (Container)boardGrid.SpawnGridObject(spawnPosition, firstChest, null);

        foreach (var item in items)
        {
            var info = new InventoryItemInfo(item);
            if (InventorySystem.Instance.AutoPlaceItem(container.GetContainer(), info))
                Debug.Log("Completed");
            else
                Debug.Log("Not placed");

        }
    }
}
