using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public List<InventoryGrid> inventoryGrids = new();

    public EquipmentController currentEquipment;

    public InventoryGrid LastGrid;

    public Vector3 LastPlacePosition;

    public InventoryItem currentItem;

    [SerializeField] private Image prefab;

    private List<Image> markedCells = new();

    [SerializeField] private Color NotPlaceable;
    [SerializeField] private Color Placeable;
    [SerializeField] private Color NotDisturb;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (currentItem == null)
            return;

        MarkPlacementPositions();
    }

    public void PickUpItem(InventoryItem inventoryItem)
    {
        currentItem = inventoryItem;

        CreateMarkingCells();

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

    public void DropItem(InventoryItem inventoryItem)
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
        var itemPosition = inventoryItem.grabbingItem.MyRectTransform.position;

        if ((gridForPlace == null || !gridForPlace.TyrPlaceItem(inventoryItem, itemPosition)) &&
         !currentEquipment.TryPlaceItem(inventoryItem, itemPosition))
            if (LastGrid != null)
                LastGrid.TyrPlaceItem(inventoryItem, LastPlacePosition);

        ClearMarkingCells();
        currentItem = null;
    }

    private void CreateMarkingCells()
    {
        int count = currentItem.Size.x * currentItem.Size.y;

        for (int i = 0; i < count; i++)
        {
            markedCells.Add(Instantiate(prefab, currentItem.grabbingItem.MyRectTransform));
        }
    }

    private void ClearMarkingCells()
    {
        for (int i = 0; i < markedCells.Count; i++)
        {
            Destroy(markedCells[i].gameObject);
        }

        markedCells.Clear();
    }

    private void MarkPlacementPositions()
    {
        var positions = currentItem.GetItemPlacePositions(currentItem.grabbingItem.MyRectTransform.position);

        for (int i = 0; i < markedCells.Count; i++)
        {
            var cell = CheckPlacementPosition(positions[i]);
            markedCells[i].color = NotDisturb;

            if (cell == null)
                continue;

            if (cell.IsOccupied)
                markedCells[i].color = NotPlaceable;
            else
                markedCells[i].color = Placeable;

            markedCells[i].rectTransform.position = cell.MyRectTransform.position;
        }
    }

    public InventoryCell CheckPlacementPosition(Vector2 positionOfCheck)
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

        if (gridForPlace != null)
            return gridForPlace.GetCellFromPosition(positionOfCheck);

        return null;

    }

}
