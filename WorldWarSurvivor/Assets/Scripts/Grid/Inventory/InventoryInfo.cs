using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInfo 
{
    public Vector2 Size;

    [SerializeField] public List<InventoryItemInfo> Items = new();
    
}
