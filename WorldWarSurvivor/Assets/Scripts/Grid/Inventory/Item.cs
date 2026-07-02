using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public const float GridCellSize = 50;

    public ItemType itemType;

    public Vector2Int Size;

    public GrabbingItem grabbingItem;

    public Direciton direciton = Direciton.Right;

    public RectTransform DebugPoint;

    public List<Vector3> GetItemPlacePositions(Vector3 ItemPosition)
    {
        List<Vector3> positions = new();
        Vector3 offset;
        
        offset = (new Vector3(-Size.x, Size.y) * (GridCellSize / 2)) + ItemPosition + new Vector3(GridCellSize / 2, -GridCellSize / 2);

        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                positions.Add(offset + new Vector3(GridCellSize * x, GridCellSize * -y, 0));
            }
        }

        return positions;
    }

    public void SetPositionReferencedByCell(Vector3 CellPosition)
    {
        grabbingItem.MyRectTransform.position = CellPosition + new Vector3(Size.x, -Size.y) * (GridCellSize / 2) + new Vector3(-GridCellSize / 2, GridCellSize / 2);
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