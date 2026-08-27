using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//Task of this class start game. If i can t play game from start, that mean class doesn t work
public class BattlefieldPreparer : MonoBehaviour
{
    public BoardGrid grid;
    public MapCreator mapCreator;

    public Vector2Int GridSize;
    public Vector2Int AccessibleCellsSize;


    //Here need change to create from one prefab and create model from ScriptableObject
    public GridObject gridObectPrefab;

    [SerializeField]
    private SelectPreparePosition selectPreparePosition;

    public PreparationWindow PreparationWindow;
    public ActionWindow ActionWindow;


    [SerializeField]
    private EnemyBand enemyBandExample;

    public bool IsCreateDummy;
    public Test deleteLaterTest;
    private int CurrentAmountOfDeadEnemies = 0;
    private int AmountOfEnemies = 0;
    public System.Action OnAllEnemiesDead;

    private void Start()
    {
        PreparationWindow.EndPreparationButton.onClick.AddListener(EndPreparation);

        selectPreparePosition.AccessibleCellsSize = AccessibleCellsSize;

        OnAllEnemiesDead += ActionWindow.Hide;
    }


    #region Battlefield creation
    //string battleObstacles = "CityObstacleData", string backgroundMap = "CityMap"
    public void CrteateFight(EnemyBand enemyBand, string battleObstacles, string backgroundMap)
    {
        #region Map creation

        CreateGrid();
        mapCreator.CreateObstacles(battleObstacles);
        mapCreator.CreateMapBackground(backgroundMap);

        #endregion

        CreateEnemyBand(enemyBand);

        HideAllCells();
        CreatePlayerBand();

        if (IsCreateDummy)
            CreateDummy();

        PreparationWindow.Show();
    }

    private void CreateGrid()
    {
        grid.CreateGrid(GridSize.x, GridSize.y);
    }
    private void CreatePlayerBand()
    {
        var accesibleCells = selectPreparePosition.FindAccessibleCellsForFirends().ToList();

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
            item.Show();

    }
    private void CreateEnemyBand(EnemyBand enemyBand = null)
    {

        CurrentAmountOfDeadEnemies = 0;
        AmountOfEnemies = 0;

        if (enemyBand == null)
            return;

        var accesibleCells = selectPreparePosition.FindAccessibleCellsForEnemies().ToList();

        foreach (var item in enemyBand.Band)
        {
            Human human;

            do
            {
                int rand = Random.Range(0, accesibleCells.Count);
                var cell = accesibleCells[rand];

                human = TeamDefiner.CreateObject(grid, cell.Coordinate, gridObectPrefab, item.GenerateHuman()) as Human;
                human.IsFriend = false;
                human.gameObject.name = "Enemy warrior";
                human.OnDestroyGridObject += CheckEnemiesCount;
                AmountOfEnemies++;

            } while (human == null);
        }
    }
    #endregion

    public void ClearBattlefield()
    {
        TurnController.ClearAllObjects();
        grid.ClearCells();
        mapCreator.ClearMap();

    }

    private void CheckEnemiesCount()
    {
        CurrentAmountOfDeadEnemies++;

        if (CurrentAmountOfDeadEnemies >= AmountOfEnemies)
            OnAllEnemiesDead?.Invoke();

    }

    private void CreateDummy()
    {
        Human human = TeamDefiner.CreateObject(grid, Vector2Int.one, gridObectPrefab, enemyBandExample.Band[0].GenerateHuman()) as Human;
        human.IsFriend = false;
        human.gameObject.name = "TrainingDummy";
    }

    private void HideAllCells()
    {
        for (int x = 0; x < grid.GridSize.x; x++)
            for (int y = 0; y < grid.GridSize.y; y++)
                grid.GetCell(x, y).FullHide();
    }

    private void EndPreparation()
    {
        PreparationWindow.Hide();
        ActionWindow.Show();

        selectPreparePosition.ClearGrid();
        selectPreparePosition.enabled = false;
        FogOfWar.UpdateAllVisibleCells(grid);

        TurnController.SetNextTurn();
    }
}
