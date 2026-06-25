using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellSelecter : MonoBehaviour
{
    public Grid grid;

    public GridObject currentObject;

    private int _currentActionIndex;

    public int CurrentActionIndex
    {
        set
        {
            _currentActionIndex = value;
            OnChangingAction?.Invoke();
        }
        get => _currentActionIndex;
    }

    private List<(Action<Cell>, HashSet<Cell>)> currentAction;

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
        if (Input.GetMouseButtonDown(0))
        {
            switch (selectRegime)
            {
                case SelectRegime.GridObjectSelect:
                    CellSelect();
                    break;
                case SelectRegime.TargetSelect:
                    CellSelectForAction();
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            selectRegime = SelectRegime.GridObjectSelect;
            ClearAccessibleCells();
            ClearSelectedGridObject();
        }
    }

    public void CellSelect()
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity) || IsPointerOverUIElement())
            return;

        var cell = grid.GetCellFromWorldPosition(hit.point);

        currentObject = cell.ShowCell();
        if (currentObject == null)
            return;

        currentObject.GetActions(out List<(Action<Cell>, HashSet<Cell>)> actions, out List<string> actionText);
        currentAction = actions;

        ActionWindow.Instance.CreateButtons(actionText);

        foreach (var accessibleCell in currentAction[CurrentActionIndex].Item2)
        {
            accessibleCell.GetComponentInChildren<MeshRenderer>().material = passMaterial;
        }

        selectRegime = SelectRegime.TargetSelect;

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


    }

    private void ClearAccessibleCells()
    {
        foreach (var accessibleCell in currentAction[CurrentActionIndex].Item2)
        {
            accessibleCell.GetComponentInChildren<MeshRenderer>().material = defaultMaterial;
        }
    }

    private void ClearSelectedGridObject()
    {
        currentObject = null;
        currentAction.Clear();
        ActionWindow.Instance.ClearActionWindow();

    }

    private void MarkAccesibleCells()
    {
        ClearAccessibleCells();

        currentObject.GetActions(out List<(Action<Cell>, HashSet<Cell>)> actions, out List<string> actionText);
        currentAction = actions;


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
