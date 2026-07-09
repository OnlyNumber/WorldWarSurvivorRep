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
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity) || IsPointerOverUIElement())
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
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity) || IsPointerOverUIElement())
            return;

        var cell = grid.GetCellFromWorldPosition(hit.point);

        if (!currentAction[CurrentActionIndex].Item2.Contains(cell))
            return;

        currentAction[CurrentActionIndex].Item1.Invoke(cell);
        //In future add delay before refreshing data
        MarkAccesibleCells();

        CurrentObject.ShowActions();

    }

    private void ClearAccessibleCells(int index)
    {
        if (currentAction[index].Item2 == null || currentAction[index].Item2.Count == 0)
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

    #region Check UI
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == (int)UILayer)
                return true;
        }
        return false;
    }

    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    #endregion

}
public enum SelectRegime
{
    GridObjectSelect,
    TargetSelect
}
