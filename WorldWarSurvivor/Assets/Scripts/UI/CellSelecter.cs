using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellSelecter : MonoBehaviour
{
    public Grid grid;

    public GridObject currentObject;

    public int currentActionIndex;

    private List<Action<Cell>> currentAction;

    public static CellSelecter Instance;

    private LayerMask UILayer;

    private SelectRegime selectRegime;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        UILayer = LayerMask.NameToLayer("UI");

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
            selectRegime = SelectRegime.TargetSelect;
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

        currentObject.GetActions(out List<Action<Cell>> actions, out List<string> actionText);

        currentAction = actions;
        ActionWindow.Instance.CreateButtons(actionText);

        selectRegime = SelectRegime.TargetSelect;

    }

    public void CellSelectForAction()
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity) || IsPointerOverUIElement())
            return;

        var cell = grid.GetCellFromWorldPosition(hit.point);

        currentAction[currentActionIndex].Invoke(cell);
    }

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

    private void ClearSelectedGridObject()
    {
        currentObject = null;
        currentAction.Clear();
        ActionWindow.Instance.ClearActionWindow();

    }

    public enum SelectRegime
    {
        GridObjectSelect,
        TargetSelect
    }
}
