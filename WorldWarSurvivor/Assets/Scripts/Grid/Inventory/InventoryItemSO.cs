using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ItemSO")]
public class InventoryItemSO : ScriptableObject
{
    public int UniqueIndex;

    [field: SerializeField] public ActionSO ActiveItem
    {
        get;
        private set;
    }

    [field: SerializeField] public Sprite ItemImage
    {
        get;
        private set;
    }

    [field: SerializeField] public ItemType itemType
    {
        get;
        private set;
    }

    [field: SerializeField] public Vector2Int Size
    {
        get;
        private set;
    }
}
