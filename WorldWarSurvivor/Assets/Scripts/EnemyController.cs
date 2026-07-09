using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance;
    
    public GridObject CurrentObject;

    public BoardGrid boardGrid;

    private void Start()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    public void SetCurrentObject(GridObject gridObject)
    {
        CurrentObject = gridObject;

        ActivateObject();
    } 

    private void ActivateObject()
    {
        var position = CurrentObject.MyCurrentCell.Coordinate;

        position += new Vector2Int(1,0);

        (CurrentObject as Human).Move(boardGrid.GetCell(position));

        StartCoroutine(CreateVisibilityOfThinking());
    } 

    private IEnumerator CreateVisibilityOfThinking()
    {
        yield return new WaitForSeconds(5);
        EndTurn();
    }

    public void EndTurn()
    {
        TurnController.SetNextTurn();
    }


}
