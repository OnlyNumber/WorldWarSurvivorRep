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

    public HumanInventoryInfo HumanInventoryInfo = new();

    public HumanStats()
    {
        HumanInventoryInfo.Size.x = 8;
        HumanInventoryInfo.Size.y = 6;
    }

}
