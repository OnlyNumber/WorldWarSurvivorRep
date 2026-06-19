using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public const float CellSquareSize = 1;

    [SerializeField]
    private Cell cellPrefab;
    public Vector2Int GridSize;

    private List<Cell> _currentCells = new();

    [ContextMenu("CreateGrid")]
    public void CreateGrid()
    {
        UnityEngine.Vector3 offset = UnityEngine.Vector3.one * CellSquareSize / 2;

        for (int y = 0; y < GridSize.y; y++)
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                UnityEngine.Vector3 cellPosition = offset + new UnityEngine.Vector3(x * CellSquareSize, 0, y * CellSquareSize);
                var cell = Instantiate(cellPrefab, cellPosition, UnityEngine.Quaternion.identity);

                cell.Initialize(new Vector2Int(x, y));
                _currentCells.Add(cell);
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        if (x < 0 || x >= GridSize.x || y < 0 || y >= GridSize.y)
            return null;

        return _currentCells[y * GridSize.x + x];
    }

    public Cell GetCell(Vector2Int coordinate)
    {
        return GetCell(coordinate.x, coordinate.y);
    }

}
