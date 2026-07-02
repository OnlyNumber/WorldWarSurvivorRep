using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGrid : Grid<InventoryCell>
{
    public List<Item> InventoryItems;

    public RectTransform rectTransform;

    public override void CreateGrid()
    {
        Vector2 offset = Vector3.one * CellSquareSize / 2;
        offset.y -= CellSquareSize;

        for (int y = 0; y < GridSize.y; y++)
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                var cell = Instantiate(cellPrefab, Vector2.zero, Quaternion.identity, rectTransform);

                Vector2 cellPosition = offset + new Vector2(x * CellSquareSize, -y * CellSquareSize);
                cell.MyRectTransform.localPosition = cellPosition;
                //cell.GetComponent<RectTransform>();

                cell.Initialize(new Vector2Int(x, y));
                currentCells.Add(cell);
            }
        }
    }

    public RectTransform pos;

    public InventoryCell GetCellFromPosition(Vector3 position)
    {
        Vector3 coordinate = position - rectTransform.position;

        coordinate /= CellSquareSize;

        return GetCell((int)coordinate.x, Mathf.Abs((int)coordinate.y));
    }

    public bool TyrPlaceItem(Item item, Vector2 position)
    {
        List<InventoryCell> inventoryCells = new();

        foreach (var itemCellPosition in item.GetItemPlacePositions(position))
        {
            var cell = GetCellFromPosition(itemCellPosition);

            if (cell == null || cell.IsOccupied)
            {
                return false;
            }
            else
                inventoryCells.Add(cell);
        }

        foreach (var invCell in inventoryCells)
        {
            invCell.IsOccupied = true;
        }

        InventoryItems.Add(item);

        item.SetPositionReferencedByCell(inventoryCells[0].MyRectTransform.position);

        return true;
    }

    public void RemoveItem(Item item)
    {
        if (!InventoryItems.Contains(item))
        {
            Debug.LogWarning("No item in inventory " + gameObject.name); 
            return;
        }

        foreach (var itemCellPosition in item.GetItemPlacePositions(item.grabbingItem.MyRectTransform.position))
        {
            var cell = GetCellFromPosition(itemCellPosition);
            cell.IsOccupied = false;
        }

        InventoryItems.Remove(item);
    }

}
