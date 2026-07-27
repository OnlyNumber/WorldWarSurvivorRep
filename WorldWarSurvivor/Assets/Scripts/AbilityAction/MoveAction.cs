using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : AbilityAction
{
    private int WalkCost;

    public override void Initialize(ActingObject actingObject, BoardGrid myGrid)
    {
        CurrentActingObject = actingObject;
        this.myGrid = myGrid;
    }

    private Human GetHuman()
    {
        return CurrentActingObject as Human;
    }

    public override void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public override HashSet<BoardCell> GetAccessibleCells(BoardGrid boardGrid, BoardCell boardCell)
    {
        HashSet<BoardCell> cells = new();

        foreach (var item in AStarPathfinding.GetReachableTiles(GetHuman().MyCurrentCell.Coordinate, GetHuman().CurrentEnergy, myGrid))
        {
            cells.Add(myGrid.GetCell(item));
        }

        return cells;
    }

    public override bool IsActionAccessible()
    {
        throw new System.NotImplementedException();
    }

    public override void TargetCell(BoardCell targetCell)
    {
        var path = AStarPathfinding.FindPath(myGrid, GetHuman().MyCurrentCell.Coordinate, targetCell.Coordinate);
        path.Remove(path[0]);

        GetHuman().ChangeEnergy(-path.Count * WalkCost);
        myGrid.TrySetGridObjectToCell(myGrid.RemoveFromGrid(GetHuman().MyCurrentCell), targetCell, false);

        //StartCoroutine(MovingAnimation(path, targetCell));
    }

    /*
    private IEnumerator MovingAnimation(List<BoardCell> cells, BoardCell endPosition)
    {
        int index = 0;

        TurnController.AddMovingObject(this);

        humanAnimator.PlayAnimation(Animations.Walk);

        do
        {
            var cellPosition = cells[index].transform.position;

            transform.position = Vector3.MoveTowards(transform.position, cellPosition, Time.deltaTime * speed);

            Vector3 direction = (cellPosition - transform.position).normalized;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);


            if (Vector3.Distance(transform.position, cellPosition) < DistanceBetweenPoints)
                index++;

            yield return null;

        } while (index < cells.Count);

        humanAnimator.PlayAnimation(Animations.Idle);
        TurnController.RemoveMovingObject(this);

    }*/

}
