using System.Collections.Generic;

using UnityEngine;

public class Grid<T> : MonoBehaviour where T : Cell
{
    [SerializeField] protected float CellSquareSize = 1;

    [SerializeField]
    protected T cellPrefab;

    public Vector2Int GridSize;

    protected List<T> currentCells = new();

    public virtual void CreateGrid(int sizeX = 1, int sizeY = 1)
    {
        if (sizeX <= 0 || sizeY <= 0)
            return;

        GridSize.x = sizeX;
        GridSize.y = sizeY;


        Vector3 offset = Vector3.one * CellSquareSize / 2;

        for (int y = 0; y < GridSize.y; y++)
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                Vector3 cellPosition = offset + new Vector3(x * CellSquareSize, 0, y * CellSquareSize);
                var cell = Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);

                cell.Initialize(new Vector2Int(x, y));
                currentCells.Add(cell);
            }
        }
    }

    public T GetCell(int x, int y)
    {
        if (x < 0 || x >= GridSize.x || y < 0 || y >= GridSize.y)
            return null;

        return currentCells[y * GridSize.x + x];
    }

    public T GetCell(Vector2Int coordinate)
    {
        return GetCell(coordinate.x, coordinate.y);
    }
}
