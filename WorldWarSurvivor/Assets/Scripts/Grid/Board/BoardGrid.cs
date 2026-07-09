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

    public GridObject SpawnGridObject(Vector2Int coordinate, GridObject gridObjectPrefab)
    {
        var cell = (BoardCell)GetCell(coordinate);

        if (cell.gridObject != null)
            return null;

        var currentObject = Instantiate(gridObjectPrefab);

        cell.gridObject = currentObject;
        cell.IsObstacle = currentObject.IsObstacle;

        currentObject.transform.position = cell.transform.position;

        currentObject.Initialize(this, cell);

        return currentObject;
    }

    public void ChangeCellOfGridObject(BoardCell fromCell, BoardCell toCell)
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
