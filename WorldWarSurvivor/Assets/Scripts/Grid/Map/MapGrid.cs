using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : Grid<MapCellRoom>
{
    public RectTransform rectTransform;

    public override void CreateGrid(int sizeX = 1, int sizeY = 1)
    {
        if (sizeX <= 0 || sizeY <= 0)
            return;

        GridSize.x = sizeX;
        GridSize.y = sizeY;

        Vector2 offset = Vector3.one * CellSquareSize / 2;
        offset.y -= CellSquareSize;

        for (int y = 0; y < GridSize.y; y++)
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                var cell = Instantiate(cellPrefab, Vector2.zero, Quaternion.identity, rectTransform);

                Vector2 cellPosition = offset + new Vector2(x * CellSquareSize, -y * CellSquareSize);
                cell.MyRectTransform.localPosition = cellPosition;

                cell.Initialize(new Vector2Int(x, y));
                currentCells.Add(cell);
            }
        }
    }

    public MapCellRoom GetCellFromWorldPosition(Vector3 position)
    {
        Vector3 coordinate = position;

        coordinate /= CellSquareSize;

        return GetCell((int)coordinate.x, (int)coordinate.z);
    }
    
}
