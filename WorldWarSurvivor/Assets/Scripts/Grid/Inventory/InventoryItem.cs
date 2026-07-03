using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public const float GridCellSize = 50;

    public ItemType itemType;

    public Vector2Int Size;

    public GrabbingItem grabbingItem;

    public Direciton direciton = Direciton.Right;

    public RectTransform DebugPoint;

    public Image Background;

    public Image ItemImage;

    private void Start()
    {
        grabbingItem.OnPickUp += OnPickUp;
        grabbingItem.OnDrop += OnDrop;

    }

    private void OnDestroy()
    {
        grabbingItem.OnPickUp -= OnPickUp;
        grabbingItem.OnDrop -= OnDrop;

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
        if (direciton == Direciton.Right || direciton == Direciton.Left)
        {

            offset = (new Vector3(-Size.x, Size.y) * (GridCellSize / 2)) + ItemPosition + new Vector3(GridCellSize / 2, -GridCellSize / 2);

            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    positions.Add(offset + new Vector3(GridCellSize * x, GridCellSize * -y, 0));
                }
            }
        }
        else
        {
            offset = (new Vector3(-Size.y, Size.x) * (GridCellSize / 2)) + ItemPosition + new Vector3(GridCellSize / 2, -GridCellSize / 2);

            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
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
        if (direciton == Direciton.Right || direciton == Direciton.Left)
            grabbingItem.MyRectTransform.position = CellPosition + new Vector3(Size.x, -Size.y) * (GridCellSize / 2) + new Vector3(-GridCellSize / 2, GridCellSize / 2);
        else
            grabbingItem.MyRectTransform.position = CellPosition + new Vector3(Size.y, -Size.x) * (GridCellSize / 2) + new Vector3(-GridCellSize / 2, GridCellSize / 2);

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