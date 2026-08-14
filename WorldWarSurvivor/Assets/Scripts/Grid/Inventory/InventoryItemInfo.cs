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

        modelController.EquipItem(inventoryItemSO.Model.modelPrefab, inventoryItemSO.Model.Place);

        if (inventoryItemSO.visualEffect == null)
            return;

        var currentItem = modelController.Find(inventoryItemSO.Model.Place);
        var currentEffect = GameObject.Instantiate(inventoryItemSO.visualEffect);

        currentEffect.Initialize(inventoryItemSO.whichAnimation, inventoryItemSO.TimeOfVisualEffect);
        currentEffect.Subscibe(modelController);
        currentItem.PlacedItemModel.CurrentEffect = currentEffect;

        if (inventoryItemSO.BodyOrItem)
            currentEffect.transform.SetParent(modelController.transform);
        else
            currentEffect.transform.SetParent(currentItem.PlacedItemModel.transform);

        currentEffect.transform.localPosition = inventoryItemSO.offsetForEffect;
        currentEffect.transform.localRotation = Quaternion.Euler(inventoryItemSO.offsetForRotation);


    }

    public void CopyInfo(InventoryItemInfo copiedInfo)
    {
        inventoryItemSO = copiedInfo.inventoryItemSO;
    }
}
