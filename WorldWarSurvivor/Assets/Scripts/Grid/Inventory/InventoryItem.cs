using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public const float GridCellSize = 50;

    public InventoryItemInfo info;

    public GrabbingItem grabbingItem;

    public Image Background;

    public Image ItemImage;

    public Action<InventoryItem> OnPickUpAction;

    public Action<InventoryItem> OnDropAction;

    public Action<InventoryItem> OnMovingAction;

    private void Start()
    {
        StartCoroutine(Utilities.WaitAndRun(Subscribe, 0.2f));
    }

    public void Initialize(InventoryItemInfo info)
    {
        this.info = info;

        Background.rectTransform.sizeDelta = (Vector2)info.Size * GridCellSize;
        ItemImage.rectTransform.sizeDelta = (Vector2)info.Size * GridCellSize;
        ItemImage.sprite = info.ItemSprite;
    }

    private void Subscribe()
    {
        OnPickUpAction += InventorySystem.Instance.PickUpItem;
        OnDropAction += InventorySystem.Instance.DropItem;

        grabbingItem.OnPickUp += OnPickUpActivation;
        grabbingItem.OnDrop += OnDropActivation;
        grabbingItem.OnMoving += OnMovingActivation;
    }

    private void Unsubscribe()
    {
        OnPickUpAction -= InventorySystem.Instance.PickUpItem;
        OnDropAction -= InventorySystem.Instance.DropItem;

        grabbingItem.OnPickUp -= OnPickUpActivation;
        grabbingItem.OnDrop -= OnDropActivation;
        grabbingItem.OnMoving -= OnMovingActivation;
    }

    private void OnPickUpActivation()
    {
        OnPickUp(this);
        OnPickUpAction?.Invoke(this);
    }

    private void OnDropActivation()
    {
        OnDrop(this);
        OnDropAction?.Invoke(this);
    }

    private void OnMovingActivation()
    {
        RotateGraggbingObject();
        OnMovingAction?.Invoke(this);
    }

    private void RotateGraggbingObject()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("RotateGraggbingObject");

            if (info.direciton == Direciton.Right)
                info.direciton = Direciton.Up;
            else
                info.direciton = Direciton.Right;

            grabbingItem.Rotate(info.direciton);

        }
    }


    private void OnPickUp(InventoryItem item)
    {
        var color = Background.color;

        color.a = 0.3f;
        Background.color = color;
        ItemImage.color = color;


    }

    private void OnDrop(InventoryItem item)
    {
        var color = Background.color;

        color.a = 1;
        Background.color = color;
        ItemImage.color = color;

    }

    public List<Vector3> GetItemPlacePositions(Vector3 ItemPosition)
    {
        List<Vector3> positions = new();
        Vector3 offset;
        if (info.direciton == Direciton.Right || info.direciton == Direciton.Left)
        {

            offset = (new Vector3(-info.Size.x, info.Size.y) * (GridCellSize / 2)) + ItemPosition + new Vector3(GridCellSize / 2, -GridCellSize / 2);

            for (int x = 0; x < info.Size.x; x++)
            {
                for (int y = 0; y < info.Size.y; y++)
                {
                    positions.Add(offset + new Vector3(GridCellSize * x, GridCellSize * -y, 0));
                }
            }
        }
        else
        {
            offset = (new Vector3(-info.Size.y, info.Size.x) * (GridCellSize / 2)) + ItemPosition + new Vector3(GridCellSize / 2, -GridCellSize / 2);

            for (int x = 0; x < info.Size.x; x++)
            {
                for (int y = 0; y < info.Size.y; y++)
                {
                    positions.Add(offset + new Vector3(GridCellSize * y, GridCellSize * -x, 0));
                    //var ppoint = Instantiate(DebugPoint);
                    //ppoint.position = offset + new Vector3(GridCellSize * y, GridCellSize * -x, 0);
                }
            }
        }


        return positions;
    }

    public void SetPositionReferencedByCell(Vector3 CellPosition)
    {
        if (info.direciton == Direciton.Right || info.direciton == Direciton.Left)
            grabbingItem.MyRectTransform.position = CellPosition + new Vector3(info.Size.x, -info.Size.y) * (GridCellSize / 2) + new Vector3(-GridCellSize / 2, GridCellSize / 2);
        else
            grabbingItem.MyRectTransform.position = CellPosition + new Vector3(info.Size.y, -info.Size.x) * (GridCellSize / 2) + new Vector3(-GridCellSize / 2, GridCellSize / 2);

    }


    private void OnDestroy()
    {
        Unsubscribe();
    }

}

public enum Direciton
{
    Up,
    Right,
    Down,
    Left
}


public enum ItemType
{
    Weapon,
    Helmet,
    BodyArmour,
    Artifact,
    QuickUseItem,
    Treasures

}