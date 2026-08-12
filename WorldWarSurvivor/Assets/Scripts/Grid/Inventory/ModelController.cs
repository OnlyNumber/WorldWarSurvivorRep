using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    [SerializeField] private HumanAnimator Animator;

    [SerializeField] private List<Position> positions;

    [SerializeField] private List<GameObject> modelRenderer;

    public void EquipItem(ItemModel itemPrefab, BodyPosition point)
    {
        var item = GameObject.Instantiate(itemPrefab);
        var part = Find(point);

        item.transform.SetParent(part.transform);
        item.EquipItem(this);
        part.PlacedItemModel = item;
    }

    public void UnEquipItem(BodyPosition point)
    {
        var position = Find(point);

        Destroy(position.PlacedItemModel.gameObject);
        position.PlacedItemModel = null;
    }

    public void ClearAllItems()
    {
        foreach (var item in positions)
        {
            if (item.PlacedItemModel != null)
            {
                item.PlacedItemModel.UnequipItem(this);
                Destroy(item.PlacedItemModel.gameObject);
            }
            item.PlacedItemModel = null;
        }
    }

    public Position Find(BodyPosition point)
    {
        foreach (var item in positions)
        {
            if (item.BodyPlace == point)
                return item;
        }

        return null;
    }

    public void SetRuntimeAnimator(RuntimeAnimatorController runtimeAnimatorController)
    {
        Animator.SetAnimator(runtimeAnimatorController);
    }

    public void SetDefaultAnimator()
    {
        Animator.SetDefaultAnimator();
    }

    public void PlayAnimation(Animations animations)
    {
        Animator.PlayAnimation(animations);
    }
    
    public void AddAnimationAction(Animations animation, float percentTime, Action action)
    {
        Animator.AddAnimationAction(animation, percentTime, action);
    }

    public void RemoveAnimationAction(Animations animation, float percentTime, Action action)
    {
        Animator.RemoveAnimationAction(animation, percentTime, action);
    }

    public void SetRendererVisibility(bool state)
    {
        foreach (var item in modelRenderer)
        {
            item.SetActive(state);
        }
    }
}

[Serializable]
public class Position
{
    public BodyPosition BodyPlace;

    public Transform transform;

    public ItemModel PlacedItemModel;
}

public enum BodyPosition
{
    Head,
    BodyItem,
    RightHand,
    LeftHand,
    Hips
}
