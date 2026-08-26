using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public const string Path_To_Data = "Assets/Resources/ScriptableObjects/MissionData";
    public const string Resources_Path_To_Data = "ScriptableObjects/MissionData";


    #region Windows 
    [SerializeField] private GameObject mapWindow;
    [SerializeField] private Button ReturnHomeButton;

    [SerializeField] private RoomEventWindow roomEventWindow;
    [SerializeField] private RewardWindow rewardWindow;
    [SerializeField] private ConfirmWindow homeReturnWindow;

    #endregion
    [SerializeField] private MissionMapController missionMapController = new();
    [SerializeField] private CommandMover commandMover;
    [SerializeField] private BattlefieldPreparer battlefieldPreparer;

    #region Map data
    [SerializeField] private EnemyBand[] enemyBands;
    [SerializeField] private string[] mapObstacles;
    [SerializeField] private string mapBackground;
    [SerializeField] private string mapName;
    #endregion

    private MissionTask _currentMissionTask;
    private MapCellRoom _currentRoom;

    private void Start()
    {
        StartCoroutine(Utilities.WaitAndRun(SetupMissionMap, 0.2f));

        battlefieldPreparer.OnAllEnemiesDead += ShowRoomReward;

        rewardWindow.GetRewardButton.onClick.AddListener(GetReward);
        rewardWindow.GetRewardButton.onClick.AddListener(BackToMapAfterFight);

        ReturnHomeButton.onClick.AddListener(ShowHomeReturnWindow);
    }

    public void SetupMissionMap()
    {

        missionMapController.SetLongOfMap(BaseProgression.Instance.PlayerData.CurrentMission.LongOfMission);
        missionMapController.CreateMap();

        mapName = GetBackgroundMap(BaseProgression.Instance.PlayerData.CurrentMission);

        enemyBands = LoadEnemyBands<EnemyBand>(Resources_Path_To_Data + "/" + mapName + "/Bands");
        mapObstacles = GetMapObstacles(Path_To_Data + "/" + mapName + "/Obstacles");
        mapBackground = Directory.GetFiles(Path_To_Data + "/" + mapName, "*.json")[0];
        _currentMissionTask = GetMission();
        _currentMissionTask = new DiscoveryRoomsMiission();
        _currentMissionTask.Initialize(this);

        commandMover.MoveToThisRoom(missionMapController.GetStartCell());
        commandMover.OnMovingToRoom += ActivateRoom;
    }

    #region  Mission Loaders
    private void SetNoActivities()
    {
        foreach (var item in missionMapController.GetAllCreatedRooms())
        {
            item.activity = Activities.Nothing;
        }
    }

    private string GetBackgroundMap(MissionData missionData)
    {
        switch (missionData.Map)
        {
            case MissionMap.Soviet:
                return "Soviet";
            case MissionMap.German:
                return "German";
            case MissionMap.American:
                return "American";
        }

        return null;
    }

    private string[] GetMapObstacles(string enemyBandsPath)
    {
        var names = Directory.GetFiles(enemyBandsPath, "*.json");

        return names;
    }

    private T[] LoadEnemyBands<T>(string enemyBandsPath) where T : UnityEngine.Object
    {
        var bands = Resources.LoadAll<T>(enemyBandsPath);

        return bands;
    }

    private MissionTask GetMission()
    {
        switch (BaseProgression.Instance.PlayerData.CurrentMission.Type)
        {
            case MissionType.KillToughEnemy:
                return new BossMission();
            case MissionType.FindSomeObjects:
                return new DiscoveryRoomsMiission();
            case MissionType.DestroyObject:
                return new DestroyObjectsMission();
        }

        return null;
    }
    #endregion

    public void AddOnMovingToRoom(System.Action<MapCellRoom> action)
    {
        commandMover.OnMovingToRoom += action;
    }

    public MissionMapController GetMissionMapController()
    {
        return missionMapController;
    }

    public void ActivateRoom(MapCellRoom room)
    {
        _currentRoom = room;
        switch (room.activity)
        {
            case Activities.Nothing:
                {
                    /*roomEventWindow.Description.text = "You see only the ruins of a past peaceful life.";

                    var choice = new Choice();
                    choice.Text = "Let's move on.";
                    choice.ChoiceAction = roomEventWindow.Hide;

                    roomEventWindow.CreateChoices(choice);
                    roomEventWindow.Show();*/
                    break;
                }
            case Activities.Battle:
                {
                    roomEventWindow.Description.text = "You see several armed people heading towards you.";

                    var choice = new Choice();
                    choice.Text = "Ready your weapons.";
                    choice.ChoiceAction = CreateRandomBattle;

                    roomEventWindow.CreateChoices(choice);
                    roomEventWindow.Show();
                    break;
                }
            case Activities.Shop:
                {
                    roomEventWindow.Description.text = "You see a traveling merchant displaying his wares.";

                    var choice = new Choice();
                    choice.Text = "Let's move on. (Work in progress)";
                    choice.ChoiceAction = roomEventWindow.Hide;

                    roomEventWindow.CreateChoices(choice);
                    roomEventWindow.Show();
                    break;
                }
            case Activities.Event:
                {
                    ////////////////FIRST EVENT///////////////
                    roomEventWindow.Description.text = "You see a tree with beautiful apples; what will you do?";

                    var choice1 = new Choice();
                    choice1.Text = "Pick the apples";
                    choice1.ChoiceAction += roomEventWindow.Hide;
                    choice1.ChoiceAction += () => _currentRoom.activity = Activities.Nothing;


                    var choice2 = new Choice();
                    choice2.Text = "Сut down a tree";
                    choice2.ChoiceAction += roomEventWindow.Hide;
                    choice2.ChoiceAction += () => _currentRoom.activity = Activities.Nothing;


                    roomEventWindow.CreateChoices(choice1, choice2);
                    roomEventWindow.Show();
                    break;
                }
            case Activities.MissionRoom:
                {
                    _currentMissionTask.ActivateMissionRoom();
                    break;
                }
        }
    }

    #region Battle
    private void CreateRandomBattle()
    {
        roomEventWindow.Hide();
        HideMap();

        var enemyBand = enemyBands[Random.Range(0, enemyBands.Length)];
        var obstacle = mapObstacles[Random.Range(0, mapObstacles.Length)];

        battlefieldPreparer.CrteateFight(enemyBand, obstacle, mapBackground);
    }

    public void CreateMissionBattle(EnemyBand enemyband)
    {
        roomEventWindow.Hide();
        HideMap();

        var obstacle = mapObstacles[Random.Range(0, mapObstacles.Length)];

        battlefieldPreparer.CrteateFight(enemyband, obstacle, mapBackground);
    }

    private void ShowRoomReward()
    {
        _currentRoom.activity = Activities.Nothing;
        rewardWindow.Show();
    }
    public void ShowMissionEnd()
    {
        ShowHomeReturnWindow();
    }

    private void GetReward()
    {
        battlefieldPreparer.ClearBattlefield();
    }

    private void BackToMapAfterFight()
    {
        battlefieldPreparer.ClearBattlefield();

        ShowMap();
        rewardWindow.Hide();
    }
    #endregion

    private void ShowMap()
    {
        mapWindow.gameObject.SetActive(true);
    }
    private void HideMap()
    {
        mapWindow.gameObject.SetActive(false);

    }

    private void BackFromMissionDefeat()
    {
        BaseProgression.Instance.SaveInfo();
        SceneManager.LoadScene(Utilities.MainMenuSceneIndex);
    }

    [ContextMenu("CheckIsAnimation")]
    private void CheckIsAnimation()
    {
        Debug.Log(TurnController.IsNowAnimation);
    }


    private void BackFromMissionWin()
    {
        BaseProgression.Instance.PlayerData.IsMissionCompleted = true;
        BaseProgression.Instance.SaveInfo();
        SceneManager.LoadScene(Utilities.MainMenuSceneIndex);
    }

    private void ShowHomeReturnWindow()
    {
        homeReturnWindow.CancelButton.onClick.RemoveAllListeners();
        homeReturnWindow.ConfirmButton.onClick.RemoveAllListeners();

        if (_currentMissionTask.IsMissionCompleted())
        {
            homeReturnWindow.Description.text = "You win, do you want get back home?";
            homeReturnWindow.CancelButton.onClick.AddListener(homeReturnWindow.Hide);
            homeReturnWindow.ConfirmButton.onClick.AddListener(BackFromMissionWin);
        }
        else
        {
            homeReturnWindow.Description.text = "Are you sure back from mission, you will not get rewards?";
            homeReturnWindow.CancelButton.onClick.AddListener(homeReturnWindow.Hide);
            homeReturnWindow.ConfirmButton.onClick.AddListener(BackFromMissionDefeat);
        }

        homeReturnWindow.Show();
    }
}
