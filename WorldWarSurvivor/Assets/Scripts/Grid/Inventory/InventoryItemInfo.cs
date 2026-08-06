using System;
using UnityEngine;

[Serializable]
public class InventoryItemInfo
{
    [Tooltip("Do not touch that (Temporary)")][SerializeField] private InventoryItemSO inventoryItemSO;

    public ActionSO AbilityActionSO => inventoryItemSO.ActiveItem;
    public bool IsItemExist => inventoryItemSO != null;
    public Sprite ItemSprite => inventoryItemSO.ItemImage;
    public ItemType itemType => inventoryItemSO.itemType;
    public Vector2Int Size => inventoryItemSO.Size;


    public Vector2Int FirstCellPosition;
    public Direction direciton = Direction.Right;

    public int inventoryItemSOIndex = -1;

    //public GameObject ItemPrefab => inventoryItemSO.ItemPrefab;

    public void GetMyItemFromIndex()
    {
        inventoryItemSO = ItemSOSearcher.GetItemFromIndex(inventoryItemSOIndex);
    }

    public void SetMyItemIndex()
    {
        inventoryItemSOIndex = inventoryItemSO.UniqueIndex;
    }

    public void EquipItem(ModelController modelController)
    {
        if (inventoryItemSO.Model == null)
            return;

        var model = GameObject.Instantiate(inventoryItemSO.Model.modelPrefab);

        modelController.EquipItem(inventoryItemSO.Model.modelPrefab, inventoryItemSO.Model.Place);
    }

}
