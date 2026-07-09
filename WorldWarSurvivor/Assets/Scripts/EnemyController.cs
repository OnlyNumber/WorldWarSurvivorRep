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

    private MeleeUnitController meleeUnitController;

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
        ActivateObject();
    }

    private void ActivateObject()
    {
        unitActions = meleeUnitController.CreateQueueOfActions();
        StartCoroutine(CreateVisibilityOfThinking());
    }

    private IEnumerator CreateVisibilityOfThinking()
    {
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
        meleeUnitController = new();

        meleeUnitController.Initialize(CurrentObject as Human, boardGrid);
    }


}
