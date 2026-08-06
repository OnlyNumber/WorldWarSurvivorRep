using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModelInfo", menuName = "ModelInfo")]
public class ModelInfo : ScriptableObject
{
    public BodyPosition Place;

    public ItemModel modelPrefab;
}
