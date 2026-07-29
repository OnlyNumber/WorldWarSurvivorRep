using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionSO : ScriptableObject
{      
    public string NameAction;


    public abstract AbilityAction GetAction();
}
