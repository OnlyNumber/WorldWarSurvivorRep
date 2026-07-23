using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGrid : Grid<BoardCell>
{
    public BoardCell GetCellFromWorldPosition(Vector3 position)
    {
        Vector3 coordinate = position;

        coordinate /= CellSquareSize;

        return GetCell((int)coordinate.x, (int)coordinate.z);
    }

    public GridObject SpawnGridObject(Vector2Int coordinate, GridObject gridObjectPrefab, bool isSpawned = false)
    {
        var cell = (BoardCell)GetCell(coordinate);

        if (cell.gridObject != null)
        {
            Debug.LogWarning(" cell.gridObject != null");
            return null;
        }

        GridObject currentObject;
        if (!isSpawned)
        {

            currentObject = Instantiate(gridObjectPrefab);
        }
        else
            currentObject = gridObjectPrefab;

        currentObject.Initialize(this, cell);

        TrySetGridObjectToCell(currentObject, cell);


        return currentObject;
    }

    public void ChangeCellOfGridObject(BoardCell fromCell, BoardCell toCell)
    {

        if (toCell.gridObject == null)
            return;

        TrySetGridObjectToCell(RemoveFromGrid(fromCell), toCell);
    }

    public GridObject RemoveFromGrid(BoardCell fromCell)
    {
        var obj = fromCell.gridObject;

        fromCell.gridObject.RemoveMyselfFromBoard();

        return obj;

    }

    public bool TrySetGridObjectToCell(GridObject gridObject, BoardCell toCell)
    {
        return gridObject.SetCurrentCells(toCell);
    }


    [ContextMenu("CreateInEditor")]
    private void CreateInEditor()
    {
        CreateGrid(GridSize.x, GridSize.y);
    }


    [ContextMenu("ClearCells")]
    public override void ClearCells()
    {
        base.ClearCells();
    }
}
