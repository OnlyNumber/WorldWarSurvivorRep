using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{

    public EquipmentSlot HeadSlot;

    public EquipmentSlot BodySlot;

    public EquipmentSlot AmuletSlot1;

    public EquipmentSlot AmuletSlot2;

    public EquipmentSlot QuickUseSlot1;

    public EquipmentSlot QuickUseSlot2;

    public EquipmentSlot MainHand;

    public EquipmentSlot OtherHand;


    private void Start()
    {

    }

    public bool TryPlaceItem(InventoryItem inventoryItem, Vector3 position)
    {
        if (inventoryItem == null)
            return false;

        foreach (var slot in BodyItems())
        {
            if (slot.inventoryItem != null || inventoryItem.info.itemType != slot.SlotType)
                continue;

            Vector3 min = new Vector3(slot.rectTransform.position.x - slot.SlotSize.x / 2, slot.rectTransform.position.y - slot.SlotSize.y / 2);
            Vector3 max = new Vector3(slot.rectTransform.position.x + slot.SlotSize.x / 2, slot.rectTransform.position.y + slot.SlotSize.y / 2);

            if (position.x > min.x && position.x <= max.x && position.y > min.y && position.y <= max.y)
            {
                slot.inventoryItem = inventoryItem;
                inventoryItem.grabbingItem.MyRectTransform.position = slot.rectTransform.position;
                return true;
            }

        }

        List<EquipmentSlot> weaponSlots = new()
        {
          MainHand,
          OtherHand
        };

        foreach (var slot in weaponSlots)
        {
            if (slot.inventoryItem != null)
                continue;

            Vector3 min = new Vector3(slot.rectTransform.position.x - slot.SlotSize.x / 2, slot.rectTransform.position.y - slot.SlotSize.y / 2);
            Vector3 max = new Vector3(slot.rectTransform.position.x + slot.SlotSize.x / 2, slot.rectTransform.position.y + slot.SlotSize.y / 2); ;

            if (position.x > min.x && position.x <= max.x && position.y > min.y && position.y <= max.y)
            {
                slot.inventoryItem = inventoryItem;
                inventoryItem.grabbingItem.MyRectTransform.position = slot.rectTransform.position;
                return true;
            }
        }

        return false;
    }

    public void RemoveItem(InventoryItem inventoryItem)
    {
        if (HeadSlot.inventoryItem == inventoryItem)
            HeadSlot.inventoryItem = null;
        if (BodySlot.inventoryItem == inventoryItem)
            BodySlot.inventoryItem = null;
        if (AmuletSlot1.inventoryItem == inventoryItem)
            AmuletSlot1.inventoryItem = null;
        if (AmuletSlot2.inventoryItem == inventoryItem)
            AmuletSlot2.inventoryItem = null;
        if (QuickUseSlot1.inventoryItem == inventoryItem)
            QuickUseSlot1.inventoryItem = null;
        if (QuickUseSlot2.inventoryItem == inventoryItem)
            QuickUseSlot2.inventoryItem = null;
        if (MainHand.inventoryItem == inventoryItem)
            MainHand.inventoryItem = null;
        if (OtherHand.inventoryItem == inventoryItem)
            OtherHand.inventoryItem = null;
    }

    private List<EquipmentSlot> BodyItems()
    {
        List<EquipmentSlot> equipmentSlots = new()
        {
            HeadSlot,
            BodySlot,
            AmuletSlot1,
            AmuletSlot2,
            QuickUseSlot1,
            QuickUseSlot2

        };


        return equipmentSlots;
    }

    public void InitializeItems(EquipmentInfo equipmentInfo)
    {
        InitializeItems(
            equipmentInfo.HeadItem,
            equipmentInfo.BodyItem,
            equipmentInfo.AmuletItem1,
            equipmentInfo.AmuletItem2,
            equipmentInfo.QuickUseItem1,
            equipmentInfo.QuickUseItem2,
            equipmentInfo.MainHandItem,
            equipmentInfo.OtherHandItem
            );
    }

    public void InitializeItems(
     InventoryItemInfo HeadSlot,
     InventoryItemInfo BodySlot,
     InventoryItemInfo AmuletSlot1,
     InventoryItemInfo AmuletSlot2,
     InventoryItemInfo QuickUseSlot1,
     InventoryItemInfo QuickUseSlot2,
     InventoryItemInfo MainHand,
     InventoryItemInfo OtherHand)
    {
        TryPlaceItem(InventorySystem.Instance.SpawnItem(HeadSlot), this.HeadSlot.rectTransform.position);
        TryPlaceItem(InventorySystem.Instance.SpawnItem(BodySlot), this.BodySlot.rectTransform.position);
        TryPlaceItem(InventorySystem.Instance.SpawnItem(AmuletSlot1), this.AmuletSlot1.rectTransform.position);
        TryPlaceItem(InventorySystem.Instance.SpawnItem(AmuletSlot2), this.AmuletSlot2.rectTransform.position);
        TryPlaceItem(InventorySystem.Instance.SpawnItem(QuickUseSlot1), this.QuickUseSlot1.rectTransform.position);
        TryPlaceItem(InventorySystem.Instance.SpawnItem(QuickUseSlot2), this.QuickUseSlot2.rectTransform.position);
        TryPlaceItem(InventorySystem.Instance.SpawnItem(MainHand), this.MainHand.rectTransform.position);
        TryPlaceItem(InventorySystem.Instance.SpawnItem(OtherHand), this.OtherHand.rectTransform.position);
    }

    public EquipmentInfo GetItems()
    {
        EquipmentInfo info = new();

        if (HeadSlot.inventoryItem != null)
            info.HeadItem = HeadSlot.inventoryItem.info;
        if (BodySlot.inventoryItem != null)
            info.BodyItem = BodySlot.inventoryItem.info;
        if (AmuletSlot1.inventoryItem != null)
            info.AmuletItem1 = AmuletSlot1.inventoryItem.info;
        if (AmuletSlot2.inventoryItem != null)
            info.AmuletItem2 = AmuletSlot2.inventoryItem.info;
        if (QuickUseSlot1.inventoryItem != null)
            info.QuickUseItem1 = QuickUseSlot1.inventoryItem.info;
        if (QuickUseSlot2.inventoryItem != null)
            info.QuickUseItem2 = QuickUseSlot2.inventoryItem.info;
        if (MainHand.inventoryItem != null)
            info.MainHandItem = MainHand.inventoryItem.info;
        if (OtherHand.inventoryItem != null)
            info.OtherHandItem = OtherHand.inventoryItem.info;

        return info;
    }

    public bool IsContainItem(InventoryItem inventoryItem)
    {
        return HeadSlot.inventoryItem == inventoryItem ||
        BodySlot.inventoryItem == inventoryItem ||
        AmuletSlot1.inventoryItem == inventoryItem ||
        AmuletSlot2.inventoryItem == inventoryItem ||
        QuickUseSlot1.inventoryItem == inventoryItem ||
        QuickUseSlot2.inventoryItem == inventoryItem ||
        MainHand.inventoryItem == inventoryItem ||
        OtherHand.inventoryItem == inventoryItem;

    }

    public void ClearEquipment()
    {
        if (HeadSlot.inventoryItem != null)
            Destroy(HeadSlot.inventoryItem.gameObject);
        if (BodySlot.inventoryItem != null)
            Destroy(BodySlot.inventoryItem.gameObject);
        if (AmuletSlot1.inventoryItem != null)
            Destroy(AmuletSlot1.inventoryItem.gameObject);
        if (AmuletSlot2.inventoryItem != null)
            Destroy(AmuletSlot2.inventoryItem.gameObject);
        if (QuickUseSlot1.inventoryItem != null)
            Destroy(QuickUseSlot1.inventoryItem.gameObject);
        if (QuickUseSlot2.inventoryItem != null)
            Destroy(QuickUseSlot2.inventoryItem.gameObject);
        if (MainHand.inventoryItem != null)
            Destroy(MainHand.inventoryItem.gameObject);
        if (OtherHand.inventoryItem != null)
            Destroy(OtherHand.inventoryItem.gameObject);
    }
}

[Serializable]
public class EquipmentSlot
{
    public RectTransform rectTransform;

    public InventoryItem inventoryItem;

    public Vector2 SlotSize;

    public ItemType SlotType;
}