using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    [SerializeField] private List<EquipmentSlot> equipmentSlots = new();

    [SerializeField] private List<EquipmentSlot> equipmentWeaponSlots = new();

    public bool TryPlaceItem(InventoryItem inventoryItem, Vector3 position)
    {
        foreach (var slot in equipmentSlots)
        {
            if (slot.inventoryItem != null || inventoryItem.info.itemType != slot.SlotType)
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

        foreach (var slot in equipmentWeaponSlots)
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

}

[Serializable]
public class EquipmentSlot
{
    public RectTransform rectTransform;

    public InventoryItem inventoryItem;

    public Vector2 SlotSize;

    public ItemType SlotType;
}