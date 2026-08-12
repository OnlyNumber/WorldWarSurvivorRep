using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectPreparePosition : MonoBehaviour
{

    public BoardGrid grid;

    [field: SerializeField]
    public GridObject CurrentObject
    {
        get;
        private set;
    }

    private HashSet<BoardCell> _accessibleCells = new();

    public Vector2Int AccessibleCellsSize;


    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material passMaterial;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CurrentObject != null)
            {
                SelectNewCell();
            }
            else
            {
                SelectTarget();
            }
        }

        if (CurrentObject != null)
        {
            WatchPlacement();
        }

    }

    private void SelectTarget()
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity) || UICheck.IsPointerOverUIElement())
            return;

        var cell = grid.GetCellFromWorldPosition(hit.point);

        if (cell == null)
            return;

        CurrentObject = grid.RemoveFromGrid(cell);

        MarkPlacement();

    }

    private void SelectNewCell()
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity) || UICheck.IsPointerOverUIElement())
            return;

        var cell = grid.GetCellFromWorldPosition(hit.point);

        if (!_accessibleCells.Contains(cell))
            return;

        grid.TrySetGridObjectToCell(CurrentObject, cell);

        CurrentObject = null;

        MarkCells(_accessibleCells, defaultMaterial);
        MarkPlacement();
    }

    private void WatchPlacement()
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity) || UICheck.IsPointerOverUIElement())
            return;

        var cell = grid.GetCellFromWorldPosition(hit.point);

        if (!_accessibleCells.Contains(cell))
            return;

        CurrentObject.transform.position = cell.transform.position;
    }

    public HashSet<BoardCell> FindAccessibleCellsForFirends()
    {
        return FindAccessibleCells(AccessibleCellsSize, 0);
    }

    public HashSet<BoardCell> FindAccessibleCellsForEnemies()
    {
        return FindAccessibleCells(AccessibleCellsSize, grid.GridSize.x - AccessibleCellsSize.x);

    }

    public HashSet<BoardCell> FindAccessibleCells(Vector2Int accesiblePositions, int startXPosition)
    {
        int middlePosition = grid.GridSize.y;

        int currentHeight = accesiblePositions.y;

        middlePosition = (middlePosition / 2) + 1;
        currentHeight /= 2;

        int first = middlePosition - currentHeight;

        HashSet<BoardCell> accessibleBoardCells = new();

        for (int y = first; y < first + accesiblePositions.y; y++)
            for (int x = startXPosition; x < startXPosition + accesiblePositions.x; x++)
                if (!grid.GetCell(x, y).IsObstacle)
                    accessibleBoardCells.Add(grid.GetCell(x, y));

        return accessibleBoardCells;

    }

    public void MarkPlacement()
    {

        _accessibleCells = FindAccessibleCellsForFirends();
        MarkCells(_accessibleCells, passMaterial);
    }

    private void MarkCells(HashSet<BoardCell> cells, Material material)
    {
        foreach (var accessibleCell in cells)
        {
            accessibleCell.GetComponentInChildren<MeshRenderer>().material = material;
        }
    }

    public void ClearGrid()
    {
        MarkCells(_accessibleCells, defaultMaterial);
    }

}
