using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HumanStats
{
    public int CurrentLevel;

    public int CurrentAmountOfExperience;

    public int HumanHealth;

    public int MeleeSkill;
    public int RangeSkill;

    public GameObject ModelPrefab;

    public List<InventoryItemInfo> items = new();

    public EquipmentInfo equipmentInfo;

}
