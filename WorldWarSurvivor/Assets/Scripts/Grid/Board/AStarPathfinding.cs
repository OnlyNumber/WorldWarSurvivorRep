using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public static class AStarPathfinding
{


    public static List<BoardCell> FindPath(BoardGrid searchingGrid, Vector2Int startPoint, Vector2Int endPoint, bool isTarget = false)
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

        PathCell currentCell = CalculateCost((BoardCell)searchingGrid.GetCell(startPoint.x, startPoint.y), endPoint, null, allCreatedCells);
        availableCells.Add(currentCell);

        do
        {

            if (availableCells.Count == 0)
                return null;

            currentCell = availableCells.First();

            foreach (var item in availableCells)
            {
                if (currentCell.FullCost > item.FullCost)
                    currentCell = item;
            }

            availableCells.Remove(currentCell);
            notavailableCells.Add(currentCell);

            List<PathCell> neighbours;

            neighbours = GetNeighbourCells(searchingGrid, currentCell.Coordinate, allCreatedCells, notavailableCells);

            foreach (var item in neighbours)
            {
                if (isTarget && item.Coordinate == endPoint)
                {
                    CalculateCost(searchingGrid.GetCell(item.Coordinate), endPoint, currentCell, allCreatedCells);
                    currentCell = item;
                    break;
                }

                if (notavailableCells.Contains(item) || searchingGrid.GetCell(item.Coordinate).IsObstacle)
                    continue;
                CalculateCost((BoardCell)searchingGrid.GetCell(item.Coordinate), endPoint, currentCell, allCreatedCells);
                availableCells.Add(item);
            }

        } while (currentCell.MyCell.Coordinate != endPoint);


        List<BoardCell> getCells = new();
        do
        {
            getCells.Add(searchingGrid.GetCell(currentCell.Coordinate));
            currentCell = currentCell.MyLastCell;

        } while (currentCell != null);

        getCells.Reverse();

        return getCells;
    }

    private static List<PathCell> GetNeighbourCells(BoardGrid searchingGrid, Vector2Int startPoint, PathCell[,] allCreatedCells, List<PathCell> notavailableCells)
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

    private static PathCell CalculateCost(BoardCell cell, Vector2Int endPoint, PathCell cellBefore, PathCell[,] allCreatedCells)
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

    public static HashSet<BoardCell> FindPossiblePositions(BoardGrid searchingGrid, Vector2Int startPoint, int maxSteps, bool isWithObstacle)
    {
        HashSet<BoardCell> visitedCells = new();
        HashSet<BoardCell> accesibleCells = new();
        Queue<BoardCell> availableCells = new();

        BoardCell currentCell = null;
        int currentStep = 0;

        availableCells.Enqueue((BoardCell)searchingGrid.GetCell(startPoint));

        do
        {

            if (availableCells.Count == 0)
            {
                foreach (var item in accesibleCells)
                    availableCells.Enqueue(item);

                accesibleCells.Clear();
                currentStep++;

            }

            if (availableCells.Count == 0 && accesibleCells.Count == 0)
                break;

            if (currentStep > maxSteps)
                break;

            currentCell = availableCells.Dequeue();
            visitedCells.Add(currentCell);

            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    int checkX = currentCell.Coordinate.x + x;
                    int checkY = currentCell.Coordinate.y + y;

                    var neighbourCell = (BoardCell)searchingGrid.GetCell(new Vector2Int(checkX, checkY));

                    if (neighbourCell == null ||
                     visitedCells.Contains(neighbourCell) ||
                      accesibleCells.Contains(neighbourCell) ||
                       neighbourCell.IsObstacle && isWithObstacle)
                        continue;


                    accesibleCells.Add(neighbourCell);
                }


        } while (currentStep <= maxSteps);

        return visitedCells;
    }

    private const int StraightCost = 10;
    private const int DiagonalCost = 14;

    /// <summary>
    /// Повертає список координат, куди юнит може дійти.
    /// </summary>
    /// <param name="start">Стартова позиція юнита</param>
    /// <param name="energy">Поточна кількість енергії</param>
    /// <param name="grid">Хеш-таблиця з координатами перешкод</param>
    public static List<Vector2Int> GetReachableTiles(Vector2Int start, int energy, BoardGrid grid, bool isWithObstacle = true)
    {
        // Словник для зберігання мінімальної енергії, витраченої на досягнення клітини
        var costSoFar = new Dictionary<Vector2Int, int>();

        // Використовуємо звичайний List замість PriorityQueue
        var frontier = new List<Vector2Int>();

        // Ініціалізація
        frontier.Add(start);
        costSoFar[start] = 0;

        // 8 напрямків руху (4 прямих, 4 діагональних)
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),   // Вгору
            new Vector2Int(0, -1),  // Вниз
            new Vector2Int(1, 0),   // Вправо
            new Vector2Int(-1, 0),  // Вліво
            new Vector2Int(1, 1),   // Вгору-вправо
            new Vector2Int(-1, 1),  // Вгору-вліво
            new Vector2Int(1, -1),  // Вниз-вправо
            new Vector2Int(-1, -1)  // Вниз-вліво
        };

        while (frontier.Count > 0)
        {
            // Шукаємо елемент із найменшою накопиченою вартістю (імітація PriorityQueue)
            int bestIndex = 0;
            for (int i = 1; i < frontier.Count; i++)
            {
                if (costSoFar[frontier[i]] < costSoFar[frontier[bestIndex]])
                {
                    bestIndex = i;
                }
            }

            // Забираємо цей елемент з "черги"
            Vector2Int current = frontier[bestIndex];
            frontier.RemoveAt(bestIndex);

            int currentCost = costSoFar[current];

            // Перевіряємо всі 8 напрямків навколо поточної клітини
            foreach (var dir in directions)
            {
                Vector2Int next = current + dir; // В Unity структури Vector2Int можна додавати математично

                // Якщо натрапили на перешкоду — ігноруємо клітину
                if (grid.GetCell(next) == null || (grid.GetCell(next).IsObstacle && isWithObstacle))
                    continue;

                // Визначаємо вартість кроку (прямо чи навскіс)
                bool isDiagonal = (dir.x != 0 && dir.y != 0);
                int moveCost = isDiagonal ? DiagonalCost : StraightCost;

                int newCost = currentCost + moveCost;

                // Якщо енергії не вистачає, щоб наступити на цю клітину, пропускаємо
                if (newCost > energy)
                    continue;

                // Якщо ми знайшли дешевший шлях до цієї клітини (або ще не відвідували її)
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;

                    // Додаємо в список для подальшої перевірки сусідів (якщо її там ще немає)
                    if (!frontier.Contains(next))
                    {
                        frontier.Add(next);
                    }
                }
            }
        }

        // Формуємо фінальний список доступних координат
        var reachableTiles = new List<Vector2Int>(costSoFar.Keys);

        // Видаляємо стартову клітину, бо юніт вже на ній стоїть
        reachableTiles.Remove(start);

        return reachableTiles;
    }

}


public class PathCell
{
    public BoardCell MyCell;

    public PathCell MyLastCell;

    public Vector2Int Coordinate;

    public float CellCost;

    public float FullCost;

    public float PassedCost;

    public float NeedToGoCost;
}