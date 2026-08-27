using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG;
using DG.Tweening;

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

    [SerializeField] private LayerMask cellLayer;

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material passMaterial;

    public Action OnChangingAction;

    [SerializeField]
    private UnitIndicator currentTargetIndicatorPrefab;

    private UnitIndicator _currentIndicator;
    private Sequence _currentSequence;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        OnChangingAction += MarkAccesibleCells;

        _currentIndicator = Instantiate(currentTargetIndicatorPrefab);
        //_currentIndicator.SetActive(false);

        TurnController.OnEndedAnimation += UpdateAfterAction;
    }

    private void UpdateAfterAction()
    {
        if (CurrentObject == null)
            return;

        MarkAccesibleCells();

        CurrentObject.ShowWindowOfUnit();
    }

    private void Update()
    {
        if (TurnController.IsNowAnimation)
            return;

        if (Input.GetMouseButtonDown(0) && CurrentObject != null)
        {

            //            Debug.Log("TurnController.IsNowAnimation " + TurnController.IsNowAnimation);
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

        gridObject.ShowWindowOfUnit();

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

        _currentIndicator.SetIndicator(gridObject.gameObject, new Vector3(0, 2f, 0));

        ShowCell(CurrentObject);
    }

    public void KillCurrentObjectIndicator()
    {
        _currentIndicator.TurnOffIndicator();
        CurrentObject = null;
    }

    public void CellSelectForAction()
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, cellLayer) || UICheck.IsPointerOverUIElement())
            return;

        var cell = grid.GetCellFromWorldPosition(hit.point);

        if (currentAction[CurrentActionIndex].Item2 != null &&
         !currentAction[CurrentActionIndex].Item2.Contains(cell))
            return;

        currentAction[CurrentActionIndex].Item1.Invoke(cell);
        //In future add delay before refreshing data
        MarkAccesibleCells();

        CurrentObject.ShowWindowOfUnit();

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

    private void OnDestroy()
    {
        _currentSequence.Kill(true);

    }

}
public enum SelectRegime
{
    GridObjectSelect,
    TargetSelect
}
