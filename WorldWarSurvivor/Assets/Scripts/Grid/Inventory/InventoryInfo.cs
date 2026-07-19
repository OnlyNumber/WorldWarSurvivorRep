using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryInfo 
{
    public Vector2Int Size;

    [SerializeField] public List<InventoryItemInfo> Items = new();
    
}
