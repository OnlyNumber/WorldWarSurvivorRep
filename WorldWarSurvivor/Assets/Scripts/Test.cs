using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform attacker;

    public Transform defender;
    public Transform Cover;

    [ContextMenu("Check")]
    public void Check()
    {
        Vector3 direction = Cover.position - defender.position;
        direction.Normalize();

        //bool isProtected = GunShootAction.CanProtectFrom(attacker.position, defender.position, direction);

        //Debug.Log("Is protecting ? " + isProtected); 
    }   
}
