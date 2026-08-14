using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ItemSO")]
public class InventoryItemSO : ScriptableObject
{

    [Tooltip("If unique index == -1, then index not initialized")] public int UniqueIndex = -1;

    [field: SerializeField]
    public ActionSO ActiveItem
    {
        get;
        private set;
    }

    [field: SerializeField]
    public Sprite ItemImage
    {
        get;
        private set;
    }

    [field: SerializeField]
    public ItemType itemType
    {
        get;
        private set;
    }

    [field: SerializeField]
    public Vector2Int Size
    {
        get;
        private set;
    }

    public ModelInfo Model;

    public VisualEffects visualEffect; 
    public Animations whichAnimation;
    public float TimeOfVisualEffect; 
    public Vector3 offsetForEffect;
    public Vector3 offsetForRotation;

    public bool BodyOrItem = true;

    [ContextMenu("GetUniqueIndex")]
    private void GetUniqueIndex()
    {

    }
}
