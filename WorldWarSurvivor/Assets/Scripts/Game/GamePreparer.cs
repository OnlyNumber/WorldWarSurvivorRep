using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Task of this class start game. If i can t play game from start, that mean class doesn t work
public class GamePreparer : MonoBehaviour
{
    public BoardGrid grid;

    public Vector2Int FriendSpawnPoint;
    public Vector2Int EnemySpawnPoint;


    public GridObject gridObectPrefab;
    public GridObject enemyGridObectPrefab;


    private void Start()
    {
        Create();

        CreateBand();
        CreateEnemyBand();

        StartCoroutine(Utilities.WaitAndRun(() => TurnController.SetNextTurn(), 0.2f));
    }

    [ContextMenu("Create")]
    private void Create()
    {
        grid.CreateGrid();
    }

    [ContextMenu("CreateBand")]
    private void CreateBand()
    {
        TeamDefiner.CreateObject(grid, FriendSpawnPoint, gridObectPrefab);
    }

    [ContextMenu("CreateEnemyBand")]
    private void CreateEnemyBand()
    {
        TeamDefiner.CreateObject(grid, EnemySpawnPoint, enemyGridObectPrefab);;
    }
}
