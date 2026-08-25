using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    public const string Path_To_Data = "Assets/Resources/ScriptableObjects/MissionData";
    public const string Resources_Path_To_Data = "ScriptableObjects/MissionData";


    #region Windows 
    [SerializeField] private GameObject mapWindow;
    [SerializeField] private RoomEventWindow roomEventWindow;
    [SerializeField] private RewardWindow rewardWindow;
    [SerializeField] private ConfirmWindow confirmWindow;
    #endregion
    [SerializeField] private MissionMapController missionMapController = new();
    [SerializeField] private CommandMover commandMover;
    [SerializeField] private BattlefieldPreparer battlefieldPreparer;

    [SerializeField] private EnemyBand[] enemyBands;
    [SerializeField] private string[] mapObstacles;
    [SerializeField] private string mapBackground;

    private string mapName;

    private MissionTask _currentMission;

    private void Start()
    {
        StartCoroutine(Utilities.WaitAndRun(SetupMissionMap, 0.2f));

        battlefieldPreparer.OnAllEnemiesDead += ShowRoomReward;

        rewardWindow.GetRewardButton.onClick.AddListener(GetReward);
        rewardWindow.GetRewardButton.onClick.AddListener(BackToMapAfterFight);

        confirmWindow.CancelButton.onClick.AddListener(confirmWindow.Hide);
        confirmWindow.ConfirmButton.onClick.AddListener(BackFromMissionDefeat);
    }

    public void SetupMissionMap()
    {

        missionMapController.SetLongOfMap(BaseProgression.Instance.PlayerData.CurrentMission.LongOfMission);
        missionMapController.CreateMap();

        mapName = GetBackgroundMap(BaseProgression.Instance.PlayerData.CurrentMission);

        enemyBands = LoadEnemyBands<EnemyBand>(Resources_Path_To_Data + "/" + mapName + "/Bands");
        mapObstacles = GetMapObstacles(Path_To_Data + "/" + mapName + "/Obstacles");
        mapBackground = Directory.GetFiles(Path_To_Data + "/" + mapName, "*.json")[0];
        _currentMission = GetMission();


        commandMover.MoveToThisRoom(missionMapController.GetStartCell());
        commandMover.OnMovingToRoom += ActivateRoom;
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
        var names = Directory.GetFiles(enemyBandsPath,"*.json");

        return names; 
    }

    private T[] LoadEnemyBands<T>(string enemyBandsPath) where T : UnityEngine.Object
    {
        var bands = Resources.LoadAll<T>(enemyBandsPath);

        return bands;
    }

    private MissionTask GetMission()
    {
        //Do something
        //enemyBands

        return null;
    }

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
        switch (room.activity)
        {
            case Activities.Nothing:
                {
                    roomEventWindow.Description.text = "You see only the ruins of a past peaceful life.";

                    var choice = new Choice();
                    choice.Text = "Let's move on.";
                    choice.ChoiceAction = roomEventWindow.Hide;

                    roomEventWindow.CreateChoices(choice);
                    roomEventWindow.Show();
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
                    choice1.ChoiceAction = roomEventWindow.Hide;

                    var choice2 = new Choice();
                    choice2.Text = "Сut down a tree";
                    choice2.ChoiceAction = roomEventWindow.Hide;

                    roomEventWindow.CreateChoices(choice1, choice2);
                    roomEventWindow.Show();
                    break;
                }
            case Activities.MissionRoom:
                {
                    _currentMission.ActivateMissionRoom();
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
        rewardWindow.Show();
    }
    public void ShowMissionEnd()
    {
        Debug.Log("ShowMissionEnd");
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
}
