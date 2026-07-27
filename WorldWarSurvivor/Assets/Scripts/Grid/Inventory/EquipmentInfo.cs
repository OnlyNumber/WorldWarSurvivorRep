using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable]
public class EquipmentInfo 
{
    public InventoryItemInfo HeadItem;

    public InventoryItemInfo BodyItem;

    public InventoryItemInfo AmuletItem1;

    public InventoryItemInfo AmuletItem2;

    public InventoryItemInfo QuickUseItem1;

    public InventoryItemInfo QuickUseItem2;

    public InventoryItemInfo MainHandItem;

    public InventoryItemInfo OtherHandItem;

    public List<InventoryItemInfo> GetAllItemsInList()
    {
        List<InventoryItemInfo> inventoryItemInfos = new()
        {
            HeadItem,
            BodyItem,
            MainHandItem,
            AmuletItem1,
            AmuletItem2,
            QuickUseItem1,
            QuickUseItem2,
            OtherHandItem
        };

        return inventoryItemInfos;
    }

}
