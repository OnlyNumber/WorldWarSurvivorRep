using UnityEngine;

public class ItemModel : MonoBehaviour
{
    public Vector3 OffsetRotation;
    public Vector3 OffsetPosition;

    public virtual void EquipItem(ModelController position)
    {
        transform.localPosition = OffsetPosition;
        transform.localRotation = Quaternion.Euler(OffsetRotation);
    }

    public virtual void UnequipItem(ModelController position)
    {
        
    }

}
