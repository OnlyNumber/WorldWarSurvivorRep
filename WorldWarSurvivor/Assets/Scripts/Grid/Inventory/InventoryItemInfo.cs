using System;
using UnityEngine;

[Serializable]
public class InventoryItemInfo
{
    [SerializeField] private InventoryItemSO inventoryItemSO;

    public Vector2Int FirstCellPosition;

    public Sprite ItemSprite => inventoryItemSO.ItemImage;

    public ItemType itemType => inventoryItemSO.itemType;

    public Vector2Int Size => inventoryItemSO.Size;
    
    public Direciton direciton = Direciton.Right;
}
