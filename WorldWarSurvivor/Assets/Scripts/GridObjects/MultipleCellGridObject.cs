using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleCellGridObject : GridObject
{
    public BoolMatrix boolMatrix;

    public Direction myDirection;

    public override void RemoveMyselfFromBoard()
    {
        throw new System.NotImplementedException();
    }

    public override bool SetCurrentCells(BoardCell cell)
    {
        var starterPoint = cell.Coordinate;
        Vector2Int currentPoint = Vector2Int.zero;
        Vector3Int direction = Utilities.DirectionToPosition(myDirection);

        for (int y = 0; y < boolMatrix.height; y++)
        {
            for (int x = 0; x < boolMatrix.width; x++)
            {
                if (boolMatrix.rows[y].cols[x])
                {
                    currentPoint.x = x;
                    currentPoint.y = y;
                    currentPoint = Utilities.SMTHAboutDirection(myDirection, currentPoint);
                    currentPoint += starterPoint;

                    myGrid.GetCell(currentPoint).IsObstacle = true;
                    myGrid.GetCell(currentPoint).gridObject = this;


                }

            }
        }
        if (gameObject.name.Contains("Damaged"))
            Debug.Log("boolMatrix.height " + boolMatrix.height);



        transform.position = cell.transform.position + ((Vector3)direction * -0.5f);
        transform.rotation = Utilities.DirectionToRotation(myDirection);

        return false;
    }
}
