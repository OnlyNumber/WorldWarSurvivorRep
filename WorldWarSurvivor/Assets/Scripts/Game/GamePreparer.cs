using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Task of this class start game. If i can t play game from start, that mean class doesn t work
public class GamePreparer : MonoBehaviour
{
    public Grid grid;

    public Vector2Int spawnPoint;

    public GridObject gridObectPrefab;

    private void Start()
    {
        Create();

        CreateBand();

        TurnController.SetNextTurn();
    }

    [ContextMenu("Create")]
    private void Create()
    {
        grid.CreateGrid();
    }

    private void CreateBand()
    {
        grid.SpawnGridObject(spawnPoint, gridObectPrefab);
    }
}
