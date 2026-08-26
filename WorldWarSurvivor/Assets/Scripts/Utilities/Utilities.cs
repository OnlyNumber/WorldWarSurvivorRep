using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public const int GameplayTestSceneIndex = 1;
    public const int MainMenuSceneIndex = 0;


    public static IEnumerator WaitAndRun(Action action, float time = 0.1f)
    {
        yield return new WaitForSeconds(time);

        action.Invoke();
    }

    public static Vector3Int DirectionToPosition(Direction dir)
    {
        switch (dir)
        {
            case Direction.Right: return new Vector3Int(1, 0, -1);
            case Direction.Down: return new Vector3Int(-1, 0, -1);
            case Direction.Left: return new Vector3Int(-1, 0, 1);
            default: return new Vector3Int(1, 0, 1); // Up
        }
    }

    public static Quaternion DirectionToRotation(Direction dir)
    {
        switch (dir)
        {
            case Direction.Right: return Quaternion.Euler(0f, 90f, 0f);
            case Direction.Down:  return Quaternion.Euler(0f, 180f, 0f);
            case Direction.Left:  return Quaternion.Euler(0f, 270f, 0f);
            default:              return Quaternion.Euler(0f, 0f, 0f); // Up
        }
    }

    public static Vector2Int SMTHAboutDirection(Direction dir, Vector2Int coordinate)
    {
        switch (dir)
        {
            case Direction.Right: return new Vector2Int(coordinate.y, -coordinate.x );
            case Direction.Down:  return new Vector2Int(-coordinate.x, -coordinate.y );
            case Direction.Left:  return new Vector2Int(-coordinate.y, coordinate.x );
            default:              return coordinate; // Up
        }
    }

    public static bool AutoPlaceItem(InventoryInfo inventory, InventoryItemInfo inventoryItem)
    {
        if (inventory == null || inventoryItem == null)
            return false;

        bool[,] cells = new bool[inventory.Size.x, inventory.Size.y];

        foreach (var item in inventory.Items)
            foreach (var cell in GetItemPositions(item.FirstCellPosition, item.Size, item.direciton))
                cells[cell.x, cell.y] = true;

        bool isFinded = true;

        Direction itemDirection = Direction.Up;

        for (int x = 0; x < inventory.Size.x; x++)
        {
            for (int y = 0; y < inventory.Size.y; y++)
            {
                isFinded = true;

                if (cells[x, y])
                    continue;

                foreach (var item in GetItemPositions(new Vector2Int(x, y), inventoryItem.Size, itemDirection))
                    if (item.x < 0 || item.x > inventory.Size.x || item.y < 0 || item.y > inventory.Size.y || cells[x, y])
                    {
                        isFinded = false;
                        break;
                    }

                if (isFinded)
                {
                    inventoryItem.FirstCellPosition = new Vector2Int(x, y);
                    inventoryItem.direciton = itemDirection;
                    inventory.Items.Add(inventoryItem);

                    return true;
                }
            }
        }

        itemDirection = Direction.Right;

        for (int x = 0; x < inventory.Size.x; x++)
        {
            for (int y = 0; y < inventory.Size.y; y++)
            {
                isFinded = true;

                if (cells[x, y])
                    continue;

                foreach (var item in GetItemPositions(new Vector2Int(x, y), inventoryItem.Size, itemDirection))
                    if (cells[x, y])
                    {
                        isFinded = false;
                        break;
                    }

                if (isFinded)
                {
                    inventoryItem.FirstCellPosition = new Vector2Int(x, y);
                    inventoryItem.direciton = itemDirection;
                    inventory.Items.Add(inventoryItem);

                    return true;

                }
            }
        }

        return false;

    }

    public static HashSet<Vector2Int> GetItemPositions(Vector2Int position, Vector2Int size, Direction direction)
    {
        HashSet<Vector2Int> cells = new();

        if (direction == Direction.Right || direction == Direction.Left)
        {
            for (int x = 0; x < size.x; x++)
                for (int y = 0; y < size.y; y++)
                    cells.Add(new Vector2Int(position.x + x, position.y + y));
        }
        else
        {
            for (int x = 0; x < size.x; x++)
                for (int y = 0; y < size.y; y++)
                    cells.Add(new Vector2Int(position.x + y, position.y + x));
        }

        return cells;
    }
}
