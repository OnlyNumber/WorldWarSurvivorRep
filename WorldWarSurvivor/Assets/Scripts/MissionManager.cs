using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{

    [SerializeField] private MissionMapController missionMapController = new();
    [SerializeField] private CommandMover commandMover;
    [SerializeField] private GameObject mapWindow;
    [SerializeField] private RoomEventWindow roomEventWindow;
    [SerializeField] private BattlefieldPreparer battlefieldPreparer;

    private void Start()
    {
        StartCoroutine(Utilities.WaitAndRun(SetupMissionMap, 0.2f));
        battlefieldPreparer.OnAllEnemiesDead += BackToMapAfterFight;
    }

    public void SetupMissionMap()
    {
        missionMapController.SetLongOfMap(BaseProgression.Instance.PlayerData.CurrentMission.LongOfMission);
        Debug.Log(BaseProgression.Instance.PlayerData.CurrentMission.LongOfMission);
        missionMapController.CreateMap();
        commandMover.MoveToThisRoom(missionMapController.GetStartCell());

        commandMover.OnMovingToRoom += ActivateRoom;
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
                    choice.ChoiceAction = CreateABattle;

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
        }
    }
    private void CreateABattle()
    {
        roomEventWindow.Hide();
        HideMap();
        battlefieldPreparer.CrteateFight();
    }

    private void BackToMapAfterFight()
    {
        battlefieldPreparer.ClearBattlefield();
        //ShowMap();

    }

    private void ShowMap()
    {
        mapWindow.gameObject.SetActive(true);
    }
    private void HideMap()
    {
        mapWindow.gameObject.SetActive(false);

    }
}
