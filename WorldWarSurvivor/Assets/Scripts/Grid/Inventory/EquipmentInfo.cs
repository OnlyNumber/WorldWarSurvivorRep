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
            /*HeadItem,
            BodyItem,
            MainHandItem,
            AmuletItem1,
            AmuletItem2,
            QuickUseItem1,
            QuickUseItem2,
            OtherHandItem*/
        };

        if (HeadItem != null)
            inventoryItemInfos.Add(HeadItem);
        if (BodyItem != null)
            inventoryItemInfos.Add(BodyItem);
        if (MainHandItem != null)
            inventoryItemInfos.Add(MainHandItem);
        if (AmuletItem1 != null)
            inventoryItemInfos.Add(AmuletItem1);
        if (AmuletItem2 != null)
            inventoryItemInfos.Add(AmuletItem2);
        if (QuickUseItem1 != null)
            inventoryItemInfos.Add(QuickUseItem1);
        if (QuickUseItem2 != null)
            inventoryItemInfos.Add(QuickUseItem2);
        if (OtherHandItem != null)
            inventoryItemInfos.Add(OtherHandItem);
        return inventoryItemInfos;
    }

}
