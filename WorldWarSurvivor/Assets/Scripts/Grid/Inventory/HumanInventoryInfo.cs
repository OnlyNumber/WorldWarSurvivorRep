using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HumanInventoryInfo : InventoryInfo
{
    [SerializeField] public EquipmentInfo EquipmentInfo = new();
}
