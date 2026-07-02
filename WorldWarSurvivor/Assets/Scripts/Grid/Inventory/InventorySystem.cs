using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public List<InventoryGrid> inventoryGrids = new();

    //public Item CurrentItem;

    public InventoryGrid LastGrid;

    public Vector3 LastPlacePosition;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PickUpItem(Item inventoryItem)
    {
        LastPlacePosition = inventoryItem.grabbingItem.MyRectTransform.position;

        foreach (var item in inventoryGrids)
        {
            if (item.InventoryItems.Contains(inventoryItem))
            {
                LastGrid = item;
                break;
            }
        }

        if (LastGrid != null)
            LastGrid.RemoveItem(inventoryItem);

    }

    public void DropItem(Item inventoryItem)
    {
        InventoryGrid gridForPlace = null;

        foreach (var item in inventoryGrids)
        {
            if (item.GetCellFromPosition(Input.mousePosition) != null)
            {
                gridForPlace = item;
                break;
            }
        }

        if (gridForPlace == null)
            return;

        if (!gridForPlace.TyrPlaceItem(inventoryItem, inventoryItem.grabbingItem.MyRectTransform.position))
        {
            Debug.Log("No place");
            /*if (LastGrid != null)
                LastGrid.TyrPlaceItem(inventoryItem, LastPlacePosition);*/

        }
        else
            Debug.Log("placed");

    }



}
