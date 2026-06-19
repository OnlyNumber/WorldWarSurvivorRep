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

                cell.Initialize(new Vector2Int(x,y));
                _currentCells.Add(cell);
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        Debug.Log("Coordinate" + _currentCells[(y) * GridSize.x + x].Coordinate);

        return _currentCells[x * GridSize.x + y];
    }


}
