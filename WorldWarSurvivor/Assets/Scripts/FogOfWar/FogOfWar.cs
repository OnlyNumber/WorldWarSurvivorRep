using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FogOfWar
{
    private const int Step_Rotation = 10;
    private const float Distange_Of_Sight = 10;
    private const float Step_Distance = 0.5f;


    private static HashSet<BoardCell> visibleCells;

    public static void UpdateAllVisibleCells(BoardGrid grid)
    {
        visibleCells.Clear();

        var friends = TurnController.GetFriendlyUnits();

        foreach (var friendlyUnit in friends)
        {
            FindVisibleCellsFromPosition(grid, (friendlyUnit as Human).MyCurrentCell.Coordinate, out var visiblePositions);

            foreach (var item in visiblePositions)
            {
                visibleCells.Add(item);
            }
        }

        

    }

    public static void FindVisibleCellsFromPosition(BoardGrid grid, Vector2Int CellPosition, out HashSet<BoardCell> visiblePositions)
    {
        visiblePositions = new();

        Vector3 center = grid.GetCell(CellPosition).transform.position;

        for (int angle = 0; angle < Step_Rotation; angle += Step_Rotation)
        {
            float rad = angle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(rad), center.y, Mathf.Sin(rad));

            for (float distance = 0; distance < Distange_Of_Sight; distance += Step_Distance)
            {
                var cell = grid.GetCellFromWorldPosition(center + direction * distance);

                if (cell == null || !cell.IsVisible)
                    break;
                
                visiblePositions.Add(cell);
            }

        }
    }
}
