using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HumanStats : GridObjectStats
{
    public int CurrentLevel;

    public int CurrentAmountOfExperience;

    public int MaxHealth;
    public int CurrentHealth;

    public int MeleeSkill;
    public int RangeSkill;

    public int SkinIndex;
    public GameObject ModelPrefab;

    public HumanInventoryInfo HumanInventoryInfo = new();

    public HumanStats()
    {
        HumanInventoryInfo.Size.x = 8;
        HumanInventoryInfo.Size.y = 6;
    }

    public List<Func<int>> numberRangeAttackEffects = new();
    public List<Func<float>> percentRangeAttackEffects = new();

    public int GetRangeAttackEffects()
    {
        int attack = 0;

        foreach (var item in numberRangeAttackEffects)
            attack += item.Invoke();

        return attack;
    }

    public float GetRangeAttackPercentEffects()
    {
        float attackPercent = 1;

        foreach (var item in percentRangeAttackEffects)
            attackPercent += item.Invoke();

        return attackPercent;
    }

    public List<Func<int>> numberRangeDefenceEffects = new();
    public List<Func<float>> percentRangeDefenceEffects = new();

    public int GetRangeDefendEffects()
    {
        int defend = 0;
        foreach (var item in numberRangeDefenceEffects)
            defend += item.Invoke();

        return defend;
    }

    public float GetRangeDefendPercentEffects()
    {
        float attackPercent = 1;

        foreach (var item in percentRangeDefenceEffects)
            attackPercent += item.Invoke();

        return attackPercent;
    }

}
