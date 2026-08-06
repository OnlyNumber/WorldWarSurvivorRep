using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemModel : ItemModel
{
    [SerializeField] private RuntimeAnimatorController runtimeAnimatorController;

    public override void EquipItem(ModelController modelController)
    {
        base.EquipItem(modelController);

        modelController.SetRuntimeAnimator(runtimeAnimatorController);
    }

    public override void UnequipItem(ModelController modelController)
    {
        base.UnequipItem(modelController);

        modelController.SetDefaultAnimator();
    }
}
