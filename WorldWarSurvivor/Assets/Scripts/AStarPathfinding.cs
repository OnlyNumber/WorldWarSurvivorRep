using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngineInternal;

public static class AStarPathfinding
{


    public static List<Cell> GetPath(Grid searchingGrid, Vector2Int startPoint, Vector2Int endPoint)
    {
        PathCell[,] allCreatedCells = new PathCell[searchingGrid.GridSize.x, searchingGrid.GridSize.y];

        for (int x = 0; x < searchingGrid.GridSize.x; x++)
        {
            for (int y = 0; y < searchingGrid.GridSize.y; y++)
            {
                allCreatedCells[x, y] = new()
                {
                    Coordinate = new Vector2Int(x, y)
                };
            }
        }


        List<PathCell> notavailableCells = new();
        HashSet<PathCell> availableCells = new();

        PathCell currentCell = CalculateCost(searchingGrid.GetCell(startPoint.x, startPoint.y), endPoint, null, allCreatedCells);
        notavailableCells.Add(currentCell);

        foreach (var item in GetNeighbourCells(searchingGrid, startPoint, allCreatedCells, notavailableCells))
        {
            if (notavailableCells.Contains(item) || searchingGrid.GetCell(item.Coordinate).IsObstacle)
                    continue;
            CalculateCost(searchingGrid.GetCell(item.Coordinate), endPoint, currentCell, allCreatedCells);
            availableCells.Add(item);
        }

        availableCells.Remove(currentCell);


        int NoInfinity = 1000;
        int index = 0;

        do
        {
            index++;

            if (index >= NoInfinity)
            {
                Debug.Log("Infinit error");
                break;
            }


            if (availableCells.Count == 0)
                break;

            currentCell = availableCells.First();

            foreach (var item in availableCells)
            {
                if (currentCell.FullCost > item.FullCost)
                    currentCell = item;
            }

            availableCells.Remove(currentCell);
            notavailableCells.Add(currentCell);


            foreach (var item in GetNeighbourCells(searchingGrid, currentCell.Coordinate, allCreatedCells, notavailableCells))
            {
                if (notavailableCells.Contains(item) || searchingGrid.GetCell(item.Coordinate).IsObstacle)
                    continue;
                CalculateCost(searchingGrid.GetCell(item.Coordinate), endPoint, currentCell, allCreatedCells);
                availableCells.Add(item);
            }

        } while (currentCell.MyCell.Coordinate != endPoint);


        List<Cell> getCells = new();
        do
        {
            getCells.Add(searchingGrid.GetCell(currentCell.Coordinate));
            currentCell = currentCell.MyLastCell;

        }while(currentCell != null);

        getCells.Reverse();

        return getCells;
    }

    private static List<PathCell> GetNeighbourCells(Grid searchingGrid, Vector2Int startPoint, PathCell[,] allCreatedCells, List<PathCell> notavailableCells)
    {
        List<PathCell> neighbourCells = new();

        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = startPoint.x + x;
                int checkY = startPoint.y + y;

                if (checkX < 0 || checkX >= searchingGrid.GridSize.x ||
                     checkY < 0 || checkY >= searchingGrid.GridSize.y)
                    continue;

                neighbourCells.Add(allCreatedCells[checkX, checkY]);
            }

        return neighbourCells;
    }

    private static PathCell CalculateCost(Cell cell, Vector2Int endPoint, PathCell cellBefore, PathCell[,] allCreatedCells)
    {
        PathCell pathCell = allCreatedCells[cell.Coordinate.x, cell.Coordinate.y];

        pathCell.MyCell = cell;

        float passedCost = 0;

        if (cellBefore != null)
        {

            Vector2 direction = cell.Coordinate - cellBefore.MyCell.Coordinate;

            if (direction.x == 0 || direction.y == 0)
                pathCell.CellCost = 10;
            else
                pathCell.CellCost = 14;

            passedCost = cellBefore.PassedCost + pathCell.CellCost;
        }

        Vector2 SmallestBiggest = endPoint - cell.Coordinate;

        SmallestBiggest.x = Mathf.Abs(SmallestBiggest.x);
        SmallestBiggest.y = Mathf.Abs(SmallestBiggest.y);

        if (SmallestBiggest.x > SmallestBiggest.y)
        {
            float temp = SmallestBiggest.x;
            SmallestBiggest.x = SmallestBiggest.y;
            SmallestBiggest.y = temp;
        }

        float NeedToGoCost = (SmallestBiggest.y - SmallestBiggest.x) * 10 + SmallestBiggest.x * 14;
        float FullCost = NeedToGoCost + passedCost;

        if (FullCost > pathCell.FullCost && pathCell.FullCost != 0)
        {
            return pathCell;
        }

        pathCell.MyLastCell = cellBefore;
        pathCell.PassedCost = passedCost;
        pathCell.NeedToGoCost = NeedToGoCost;
        pathCell.FullCost = FullCost;

        cell.FullCost.text = pathCell.FullCost.ToString();
        cell.NeedToGoCost.text = pathCell.NeedToGoCost.ToString();
        cell.PassedCost.text = pathCell.PassedCost.ToString();

        return pathCell;
    }
}


public class PathCell
{
    public Cell MyCell;

    public PathCell MyLastCell;

    public Vector2Int Coordinate;

    public float CellCost;

    public float FullCost;

    public float PassedCost;

    public float NeedToGoCost;
}