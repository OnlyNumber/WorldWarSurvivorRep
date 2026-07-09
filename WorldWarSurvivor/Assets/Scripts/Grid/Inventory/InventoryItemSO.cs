using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ItemSO")]
public class InventoryItemSO : ScriptableObject
{
    public Sprite ItemImage;

    public ItemType itemType;

    public Vector2Int Size;
}
