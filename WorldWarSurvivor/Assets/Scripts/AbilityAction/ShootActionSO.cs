using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShootActionSO", menuName = "Actioons/ShootActionSO")]
public class ShootActionSO : ActionSO
{
    public int Damage = 1;
    public int AttackRange = 10;
    public int AttackEnergyCost = 20;
    
    public VisualEffects ShootParticlePrefab;

    public override AbilityAction GetAction()
    {
        return new GunShootAction(this);
    }
}
