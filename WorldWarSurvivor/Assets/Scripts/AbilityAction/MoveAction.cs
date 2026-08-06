using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : AbilityAction
{
    private int WalkCost = 10;

    private Human CurrentHuman;

    private Transform cachedTransform;

    private float Speed = 1;

    private const float DistanceBetweenPoints = 0.1f;

    public MoveAction(ActionSO actionSO) : base(actionSO)
    {
        Speed = (actionSO as MoveActionSO).speed;
    }

    public override void Initialize(ActingObject actingObject, BoardGrid myGrid)
    {
        CurrentActingObject = actingObject;
        this.myGrid = myGrid;

        CurrentHuman = CurrentActingObject as Human;
        cachedTransform = CurrentHuman.transform;
    }

    public override void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public override HashSet<BoardCell> GetAccessibleCells()
    {
        HashSet<BoardCell> cells = new();

        foreach (var item in AStarPathfinding.GetReachableTiles(CurrentHuman.MyCurrentCell.Coordinate, CurrentHuman.CurrentEnergy, myGrid))
        {
            cells.Add(myGrid.GetCell(item));
        }

        return cells;
    }

    public override bool IsActionAccessible()
    {
        return CurrentHuman.CurrentEnergy > WalkCost;
    }

    public override void TargetCell(BoardCell targetCell)
    {
        var path = AStarPathfinding.FindPath(myGrid, CurrentHuman.MyCurrentCell.Coordinate, targetCell.Coordinate);
        path.Remove(path[0]);

        Debug.Log(path.Count);
        CurrentHuman.ChangeEnergy(-path.Count * WalkCost);
        myGrid.TrySetGridObjectToCell(myGrid.RemoveFromGrid(CurrentHuman.MyCurrentCell), targetCell, false);

        CurrentHuman.StartCoroutine(MovingAnimation(path, targetCell));
    }


    private IEnumerator MovingAnimation(List<BoardCell> cells, BoardCell endPosition)
    {
        int index = 0;

        TurnController.AddMovingObject(CurrentHuman);

        CurrentHuman.SetCurrentAnimation(Animations.Walk);

        do
        {
            var cellPosition = cells[index].transform.position;

            cachedTransform.position = Vector3.MoveTowards(cachedTransform.position, cellPosition, Time.deltaTime * Speed);

            Vector3 direction = (cellPosition - cachedTransform.position).normalized;
            if (direction != Vector3.zero)
                cachedTransform.rotation = Quaternion.LookRotation(direction);


            if (Vector3.Distance(cachedTransform.position, cellPosition) < DistanceBetweenPoints)
                index++;

            yield return null;

        } while (index < cells.Count);

        CurrentHuman.SetCurrentAnimation(Animations.Idle);
        TurnController.RemoveMovingObject(CurrentHuman);

    }

}
