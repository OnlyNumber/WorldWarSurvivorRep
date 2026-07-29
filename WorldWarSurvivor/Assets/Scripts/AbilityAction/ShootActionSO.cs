using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShootActionSO", menuName = "Actioons/ShootActionSO")]
public class ShootActionSO : ActionSO
{
    public override AbilityAction GetAction()
    {
        return new GunShootAction();
    }
}
