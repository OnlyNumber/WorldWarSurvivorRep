using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//Task of this class start game. If i can t play game from start, that mean class doesn t work
public class GamePreparer : MonoBehaviour
{
    public BoardGrid grid;
    public MapCreator mapCreator;

    public Vector2Int GridSize;
    public Vector2Int AccessibleCellsSize;


    public GridObject gridObectPrefab;
    public GridObject enemyGridObectPrefab;

    [SerializeField]
    private SelectPreparePosition selectPreparePosition;

    public GameObject PreparationWindow;
    public Button EndPreparationButton;



    private void Start()
    {
        EndPreparationButton.onClick.AddListener(EndPreparation);

        selectPreparePosition.AccessibleCellsSize = AccessibleCellsSize;
        #region Map creation
        
        CreateGrid();
        mapCreator.Create("CityObstacleData");
        mapCreator.LoadMapFromJson("CityMap");
        
        #endregion

        CreatePlayerBand();


    }

    [ContextMenu("Create")]
    private void CreateGrid()
    {
        grid.CreateGrid(GridSize.x, GridSize.y);
    }

    [ContextMenu("CreateBand")]
    private void CreatePlayerBand()
    {
        var accesibleCells = selectPreparePosition.FindAccessibleCells().ToList();

        foreach (var item in BaseProgression.Instance.PlayerData.CurrentCommand)
        {
            Human human;

            do
            {
                int rand = Random.Range(0, accesibleCells.Count);
                var cell = accesibleCells[rand];
                human = TeamDefiner.CreateObject(grid, cell.Coordinate, gridObectPrefab, item) as Human;

            } while (human == null);
        }

        selectPreparePosition.MarkPlacement();
        
        foreach (var item in accesibleCells)
        {
            item.ShowCell();
        }

    }

    [ContextMenu("CreateEnemyBand")]
    private void CreateEnemyBand()
    {
        //TeamDefiner.CreateObject(grid, EnemySpawnPoint, enemyGridObectPrefab); ;
    }

    private void EndPreparation()
    {
        PreparationWindow.SetActive(false);

        selectPreparePosition.ClearGrid();
        selectPreparePosition.enabled = false;

        TurnController.SetNextTurn();
    }
}
