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

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material passMaterial;

    public Action OnChangingAction;

    [SerializeField]
    private GameObject currentTargetIndicatorPrefab;

    private GameObject _currentIndicator;
    Sequence currentSequence;

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
        _currentIndicator.SetActive(false);

        currentSequence = DOTween.Sequence();

        currentSequence
        .Append(_currentIndicator.transform.DOLocalMoveY(_currentIndicator.transform.localPosition.y + 0.5f, 0.5f))
        .Append(_currentIndicator.transform.DOLocalMoveY(_currentIndicator.transform.localPosition.y - 0.5f, 0.5f))
        .SetLoops(-1, LoopType.Restart);
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


        _currentIndicator.SetActive(true);
        _currentIndicator.transform.parent = CurrentObject.transform;
        _currentIndicator.transform.localPosition = new Vector3(0, 2f, 0);

        currentSequence = DOTween.Sequence();

        float up = _currentIndicator.transform.localPosition.y + 0.3f;
        float down = _currentIndicator.transform.localPosition.y;


        currentSequence
        .SetLoops(-1, LoopType.Restart)
        .Append(_currentIndicator.transform.DOLocalMoveY(up, 1f).SetEase(Ease.Linear))
        .Append(_currentIndicator.transform.DOLocalMoveY(down, 1f).SetEase(Ease.Linear))
        ;
        //.Append(_currentIndicator.transform.DOLocalMoveY(_currentIndicator.transform.localPosition.y, 0.5f))

        ShowCell(CurrentObject);
    }

    public void NoCurrentObject()
    {
        currentSequence.Kill();

        _currentIndicator.SetActive(false);
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
