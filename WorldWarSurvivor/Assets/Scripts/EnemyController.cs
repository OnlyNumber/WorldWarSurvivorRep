using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance;

    public GridObject CurrentObject;

    public BoardGrid boardGrid;

    private UnitController _unitController;

    List<Action> unitActions = new();

    private void Start()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    public void SetCurrentObject(GridObject gridObject)
    {
        CurrentObject = gridObject;
        DefineAI();
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {

        yield return new WaitForSeconds(1);
        ActivateObject();
    }

    private void ActivateObject()
    {
        unitActions = _unitController.CreateQueueOfActions();
        StartCoroutine(CreateVisibilityOfThinking());
    }

    private IEnumerator CreateVisibilityOfThinking()
    {
        if (unitActions == null || unitActions.Count == 0)
            EndTurn();
            
        int index = 0;

        do
        {
            unitActions[index].Invoke();

            yield return new WaitUntil(check);

            index++;

        } while (index < unitActions.Count);

        EndTurn();
    }

    private bool check() { return !TurnController.IsNowAnimation; }

    public void EndTurn()
    {
        unitActions.Clear();
        TurnController.SetNextTurn();
    }

    private void DefineAI()
    {
        //In future define will be more complex
        if (CurrentObject.gameObject.name.Contains("Dummy"))
            _unitController = new TrainingDummy();
        else
            _unitController = new RangeUnitController();

        _unitController.Initialize(CurrentObject as Human, boardGrid);
    }


}
