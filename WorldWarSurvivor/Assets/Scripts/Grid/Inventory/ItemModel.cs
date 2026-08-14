using UnityEngine;

public class ItemModel : MonoBehaviour
{
    public Vector3 OffsetRotation;
    public Vector3 OffsetPosition;

    public VisualEffects CurrentEffect;

    public virtual void EquipItem(ModelController controller)
    {
        transform.localPosition = OffsetPosition;
        transform.localRotation = Quaternion.Euler(OffsetRotation);
    }

    public virtual void UnequipItem(ModelController controller)
    {
        if (CurrentEffect == null)
            return;

        CurrentEffect.Unsubscribe(controller);
        Destroy(CurrentEffect.gameObject);
    }

}
