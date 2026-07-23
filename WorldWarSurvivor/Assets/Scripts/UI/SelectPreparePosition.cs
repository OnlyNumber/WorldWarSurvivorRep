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

        CurrentObject = grid.RemoveFromGrid(cell);

        _accessibleCells = FindAccessibleCells();
        MarkCells(_accessibleCells, passMaterial);

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

        _accessibleCells = FindAccessibleCells();
        MarkCells(_accessibleCells, passMaterial);
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

    public HashSet<BoardCell> FindAccessibleCells()
    {
        int height = grid.GridSize.y;

        int currentHeight = AccessibleCellsSize.y;

        height = (height / 2) + 1;
        currentHeight /= 2;

        int first = height - currentHeight;

        HashSet<BoardCell> accessibleBoardCells = new();

        for (int y = first; y < first + AccessibleCellsSize.y; y++)
            for (int x = 0; x < AccessibleCellsSize.x; x++)
                if (!grid.GetCell(x, y).IsObstacle)
                    accessibleBoardCells.Add(grid.GetCell(x, y));

        return accessibleBoardCells;

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
