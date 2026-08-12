using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBand", menuName = "EnemyUnits/EnemyBand")]
public class EnemyBand : ScriptableObject
{
    public List<UnitInfo> Band;
}
