using System.Collections.Generic;

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
        Vector3 offset = Vector3.one * CellSquareSize / 2;

        for (int y = 0; y < GridSize.y; y++)
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                Vector3 cellPosition = offset + new Vector3(x * CellSquareSize, 0, y * CellSquareSize);
                var cell = Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);

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

    public Cell GetCellFromWorldPosition(Vector3 position)
    {
        Vector3 coordinate = position;

        coordinate /= CellSquareSize;

        return GetCell((int)coordinate.x, (int)coordinate.z);
    }

    public void SpawnGridObject(Vector2Int coordinate, GridObject gridObjectPrefab)
    {
        var cell = GetCell(coordinate);

        if (cell.gridObject != null)
            return;

        var currentObject = Instantiate(gridObjectPrefab);

        cell.gridObject = currentObject;
        cell.IsObstacle = currentObject.IsObstacle;

        currentObject.transform.position = cell.transform.position;

        currentObject.Initialize(this, cell);
    }

    public void ChangeCellOfGridObject(Cell fromCell, Cell toCell)
    {
        fromCell.IsObstacle = false;
        toCell.gridObject = fromCell.gridObject;
        fromCell.gridObject = null;

        if (toCell.gridObject == null)
            return;

        toCell.gridObject.MyCurrentCell = toCell;
        toCell.IsObstacle = toCell.gridObject.IsObstacle;
    }

}
