using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FogOfWar
{
    private const int Degrees = 360;
    private const int Step_Rotation = 5;
    private const float Distange_Of_Sight = 10;
    private const float Step_Distance = 0.5f;


    private static HashSet<BoardCell> visibleCells = new();

    public static void UpdateAllVisibleCells(BoardGrid grid)
    {
        foreach (var item in visibleCells)
            item.Hide();

        visibleCells.Clear();

        var friends = TurnController.GetFriendlyUnits();

        foreach (var friendlyUnit in friends)
        {
            FindVisibleCellsFromPosition(grid, (friendlyUnit as Human).MyCurrentCell.Coordinate, out var visiblePositions);

            foreach (var item in visiblePositions)
            {
                item.Show();
                visibleCells.Add(item);
            }
        }



    }

    public static void FindVisibleCellsFromPosition(BoardGrid grid, Vector2Int CellPosition, out HashSet<BoardCell> visiblePositions, float distanceOfSight = Distange_Of_Sight)
    {
        visiblePositions = new();

        Vector3 center = grid.GetCell(CellPosition).transform.position;

        for (int angle = 0; angle < Degrees; angle += Step_Rotation)
        {
            float rad = angle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(rad), center.y, Mathf.Sin(rad));

            for (float distance = 0; distance < distanceOfSight; distance += Step_Distance)
            {
                var cell = grid.GetCellFromWorldPosition(center + direction * distance);

                if (cell == null || !cell.IsVisible)
                    break;

                visiblePositions.Add(cell);
            }

        }
    }

    public static List<GameObject> positions(Vector3 center)
    {
        List<GameObject> directions = new();
        for (int angle = 0; angle < Degrees; angle += Step_Rotation)
        {
            float rad = angle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(rad), center.y, Mathf.Sin(rad));
            var obj = new GameObject();

            obj.transform.position = center + direction * 3;

            directions.Add(obj);
        }

        return directions;

    }
}
