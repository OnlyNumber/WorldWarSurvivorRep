using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealActionSO", menuName = "Actioons/HealActionSO")]
public class HealActionSO : ActionSO
{
    public override AbilityAction GetAction()
    {
        return new HealAction();
    }
}
