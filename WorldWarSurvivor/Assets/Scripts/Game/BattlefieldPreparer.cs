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

    public GameObject PreparationWindow;
    public Button EndPreparationButton;

    [SerializeField]
    private EnemyBand enemyBand;

    public bool IsCreateDummy;
    public Test deleteLaterTest;
    private int CurrentAmountOfDeadEnemies = 0;
    private int AmountOfEnemies = 0;
    public System.Action OnAllEnemiesDead;

    private void Start()
    {
        EndPreparationButton.onClick.AddListener(EndPreparation);

        selectPreparePosition.AccessibleCellsSize = AccessibleCellsSize;
    }

    #region Battlefield creation
    public void CrteateFight()
    {
        #region Map creation

        CreateGrid();
        mapCreator.Create("CityObstacleData");
        mapCreator.LoadMapFromJson("CityMap");

        #endregion

        CreateEnemyBand();

        HideAllCells();
        CreatePlayerBand();


        if (IsCreateDummy)
            CreateDummy();
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
        {
            item.Show();
        }

    }
    private void CreateEnemyBand()
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
        
    }

    private void CheckEnemiesCount()
    {
        CurrentAmountOfDeadEnemies++;

        if (CurrentAmountOfDeadEnemies >= AmountOfEnemies)
            OnAllEnemiesDead?.Invoke();

    }

    private void CreateDummy()
    {
        Human human = TeamDefiner.CreateObject(grid, Vector2Int.one, gridObectPrefab, enemyBand.Band[0].GenerateHuman()) as Human;
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
        PreparationWindow.SetActive(false);

        selectPreparePosition.ClearGrid();
        selectPreparePosition.enabled = false;
        FogOfWar.UpdateAllVisibleCells(grid);

        TurnController.SetNextTurn();
    }
}
