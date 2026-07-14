using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellSelecter : MonoBehaviour
{
    public BoardGrid grid;

    [field: SerializeField]
    public GridObject CurrentObject
    {
        get;
        private set;
    }

    private int _lastActionIndex = 0;

    private int _currentActionIndex = 0;

    public int CurrentActionIndex
    {
        set
        {
            _currentActionIndex = value;
            OnChangingAction?.Invoke();
        }
        get => _currentActionIndex;
    }

    private List<(Action<BoardCell>, HashSet<BoardCell>)> currentAction;

    public static CellSelecter Instance;

    private LayerMask UILayer;

    private SelectRegime selectRegime;

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material passMaterial;

    public Action OnChangingAction;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        UILayer = LayerMask.NameToLayer("UI");

        OnChangingAction += MarkAccesibleCells;
    }

    private void Update()
    {
        if (TurnController.IsNowAnimation)
            return;

        if (Input.GetMouseButtonDown(0) && CurrentObject != null)
        {
            //switch (selectRegime)
            //{
            /*case SelectRegime.GridObjectSelect:
                CellSelect();
                break;*/
            //case SelectRegime.TargetSelect:
            CellSelectForAction();
            //   break;
            //}
        }

        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            selectRegime = SelectRegime.GridObjectSelect;
            ClearAccessibleCells(CurrentActionIndex);
            ClearSelectedGridObject();
        }*/
    }

    private void CellSelect()
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity) || UICheck.IsPointerOverUIElement())
            return;

        var cell = grid.GetCellFromWorldPosition(hit.point);

        CurrentObject = cell.ShowCell();
        ShowCell(CurrentObject);
    }

    private void ShowCell(GridObject gridObject)
    {
        if (gridObject == null)
            return;

        gridObject.ShowActions();

        gridObject.GetActions(out List<(Action<BoardCell>, HashSet<BoardCell>)> actions, out List<string> actionText);
        currentAction = actions;

        foreach (var accessibleCell in currentAction[CurrentActionIndex].Item2)
        {
            accessibleCell.GetComponentInChildren<MeshRenderer>().material = passMaterial;
        }
    }

    public void SetCurrentObject(GridObject gridObject)
    {
        CurrentObject = gridObject;
        ShowCell(CurrentObject);
    }

    public void NoCurrentObject()
    {
        CurrentObject = null;
    }

    public void CellSelectForAction()
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity) || UICheck.IsPointerOverUIElement())
            return;

        var cell = grid.GetCellFromWorldPosition(hit.point);

        if (!currentAction[CurrentActionIndex].Item2.Contains(cell))
            return;

        currentAction[CurrentActionIndex].Item1.Invoke(cell);
        //In future add delay before refreshing data
        MarkAccesibleCells();

        CurrentObject.ShowActions();

    }

    public void ClearCurrentCells()
    {
        ClearAccessibleCells(CurrentActionIndex);
    }

    private void ClearAccessibleCells(int index)
    {

        if (currentAction == null || currentAction.Count == 0 || currentAction[index].Item2 == null || currentAction[index].Item2.Count == 0)
            return;

        foreach (var accessibleCell in currentAction[index].Item2)
        {
            accessibleCell.GetComponentInChildren<MeshRenderer>().material = defaultMaterial;
        }
    }

    private void ClearSelectedGridObject()
    {
        CurrentObject = null;
        currentAction.Clear();
        ActionWindow.Instance.ClearActionWindow();

    }

    private void MarkAccesibleCells()
    {
        ClearAccessibleCells(_lastActionIndex);

        _lastActionIndex = CurrentActionIndex;

        CurrentObject.GetActions(out List<(Action<BoardCell>, HashSet<BoardCell>)> actions, out List<string> actionText);
        currentAction = actions;

        if (currentAction[CurrentActionIndex].Item2 == null || currentAction[CurrentActionIndex].Item2.Count == 0)
            return;

        foreach (var accessibleCell in currentAction[CurrentActionIndex].Item2)
        {
            accessibleCell.GetComponentInChildren<MeshRenderer>().material = passMaterial;
        }
    }

}
public enum SelectRegime
{
    GridObjectSelect,
    TargetSelect
}
