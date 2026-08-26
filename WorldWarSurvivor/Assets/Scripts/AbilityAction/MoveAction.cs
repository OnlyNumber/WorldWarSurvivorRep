using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveAction : AbilityAction
{
    private int StraightWalkCosst = 10;
    private int DiagonalWalkCost = 14;

    private Human CurrentHuman;

    private Transform cachedTransform;

    private float Speed = 20;

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

    public override HashSet<BoardCell> GetAccessibleCells(Vector2Int CellPosition)
    {
        HashSet<BoardCell> cells = new();

        foreach (var item in AStarPathfinding.GetReachableTiles(CellPosition, CurrentHuman.CurrentEnergy, myGrid))
        {
            cells.Add(myGrid.GetCell(item));
        }

        return cells;
    }

    public override HashSet<BoardCell> GetAccessibleCells()
    {
        return GetAccessibleCells(CurrentHuman.MyCurrentCell.Coordinate);
    }

    public override bool IsActionAccessible()
    {
        return CurrentHuman.CurrentEnergy > StraightWalkCosst;
    }

    public override void TargetCell(BoardCell targetCell)
    {
        var path = AStarPathfinding.FindPath(myGrid, CurrentHuman.MyCurrentCell.Coordinate, targetCell.Coordinate);
        path.Remove(path[0]);

        int diagonalCells = 0;
        Cell compareCell = path.First();

        foreach (var item in path)
        {
            if (Abs(item.Coordinate - compareCell.Coordinate) == Vector2Int.one)
                diagonalCells++;

            compareCell = item;
        }

        Debug.Log("diagonal " + diagonalCells);

        int totalCost = diagonalCells * DiagonalWalkCost + (path.Count - diagonalCells) * StraightWalkCosst;

        CurrentHuman.ChangeEnergy(-totalCost);

        myGrid.TrySetGridObjectToCell(myGrid.RemoveFromGrid(CurrentHuman.MyCurrentCell), targetCell, false);

        CurrentHuman.StartCoroutine(MovingAnimation(path, targetCell));

        Vector2Int Abs(Vector2Int vector2Int)
        {
            int x = Mathf.Abs(vector2Int.x);
            int y = Mathf.Abs(vector2Int.y);

            return new Vector2Int(x, y);
        }
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

        OnEndAbilityAction?.Invoke();

    }
}
