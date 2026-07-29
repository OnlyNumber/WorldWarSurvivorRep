using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MoveActionSO", menuName = "Actioons/MoveActionSO")]
public class MoveActionSO : ActionSO
{
    [SerializeField] private float speed;

    public override AbilityAction GetAction()
    {
        return new MoveAction();
    }
}