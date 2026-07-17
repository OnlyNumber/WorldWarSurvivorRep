using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionControl : MonoBehaviour
{
    private Dictionary<HumanStats, HumanUIButton> AllHumansUI = new();

    public GameObject ExpeditiontWindow;

    public ChooseMap ChooseMapWindow;

    public Camera ExpeditionCamera;

    #region Lists 
    public RectTransform RosterLists;
    public RectTransform CommandLists;
    public RectTransform TransferParent;
    public HumanUIButton HumanUIPrefab;
    #endregion

    #region  buttons
    public Button StartExpeditionButton;
    public Button CloseButton;
    #endregion

    private List<HumanStats> _lastProgressionList;
    private RectTransform _lastTransformList;

    private void Start()
    {
        CloseButton.onClick.AddListener(CloseExpedition);
        CloseButton.onClick.AddListener(() => CameraControl.ChangeToCamera(CameraControl.MainCamera));


        StartExpeditionButton.onClick.AddListener(CloseExpedition);
        StartExpeditionButton.onClick.AddListener(ChooseMap);

        
        ChooseMapWindow.CloseButton.onClick.AddListener(ChooseMap);
        ChooseMapWindow.CloseButton.onClick.AddListener(() => CameraControl.ChangeToCamera(ExpeditionCamera));


    }

    public void OpenExpedition()
    {
        ExpeditiontWindow.SetActive(true);
        Initialize();
    }

    private void CloseExpedition()
    {
        ExpeditiontWindow.SetActive(false);

        ClearWindow();
    }

    private void ChooseMap()
    {
        ChooseMapWindow.OpenExpedition();
        CameraControl.ChangeToCamera(ChooseMapWindow.ChooseMapCamera);

    }

    public void Initialize()
    {
        for (int i = 0; i < BaseProgression.Instance.Rostert.Count; i++)
        {
            var generatedHuman = BaseProgression.Instance.Rostert[i];

            var humanUI = Instantiate(HumanUIPrefab, RosterLists);

            humanUI.CurrentLevel.text = generatedHuman.CurrentLevel.ToString();
            humanUI.HumanHealth.text = generatedHuman.HumanHealth.ToString();

            humanUI.MeleeSkill.text = generatedHuman.MeleeSkill.ToString();
            humanUI.RangeSkill.text = generatedHuman.RangeSkill.ToString();


            humanUI.GrabbingItem.OnPickUp += () => TakeItem(generatedHuman);

            humanUI.GrabbingItem.OnDrop += () => TryAddHumanToCommand(generatedHuman, humanUI, GetPosition());

            AllHumansUI.Add(generatedHuman, humanUI);
        }

        for (int i = 0; i < BaseProgression.Instance.CurrentCommand.Count; i++)
        {
            var generatedHuman = BaseProgression.Instance.CurrentCommand[i];

            var humanUI = Instantiate(HumanUIPrefab, CommandLists);

            humanUI.CurrentLevel.text = generatedHuman.CurrentLevel.ToString();
            humanUI.HumanHealth.text = generatedHuman.HumanHealth.ToString();

            humanUI.MeleeSkill.text = generatedHuman.MeleeSkill.ToString();
            humanUI.RangeSkill.text = generatedHuman.RangeSkill.ToString();

            humanUI.GrabbingItem.OnPickUp += () => TakeItem(generatedHuman);

            humanUI.GrabbingItem.OnDrop += () => TryAddHumanToCommand(generatedHuman, humanUI, GetPosition());


            AllHumansUI.Add(generatedHuman, humanUI);
        }

    }

    private Vector2 GetPosition()
    {
        return Input.mousePosition;
    }

    private void TakeItem(HumanStats stats)
    {
        var humanUI = AllHumansUI[stats];

        humanUI.transform.SetParent(TransferParent);

        if (BaseProgression.Instance.Rostert.Contains(stats))
        {
            _lastProgressionList = BaseProgression.Instance.Rostert;
            BaseProgression.Instance.Rostert.Remove(stats);
        }
        else
        {
            _lastProgressionList = BaseProgression.Instance.CurrentCommand;
            BaseProgression.Instance.CurrentCommand.Remove(stats);
        }
    }

    private void TryAddHumanToCommand(HumanStats stats, HumanUIButton humanUI, Vector2 position)
    {
        if (UtilitiesUI.IsInsideOfSquareImage(CommandLists.position, CommandLists.rect, position))
            SetHumanUI(BaseProgression.Instance.CurrentCommand, CommandLists);
        else if (UtilitiesUI.IsInsideOfSquareImage(RosterLists.position, RosterLists.rect, position))
            SetHumanUI(BaseProgression.Instance.Rostert, RosterLists);
        else
            SetHumanUI(_lastProgressionList, _lastTransformList);




        void SetHumanUI(List<HumanStats> list, RectTransform parent)
        {
            list.Add(stats);
            humanUI.transform.SetParent(parent);
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
        }
    }

    private void ClearWindow()
    {
        foreach (var item in AllHumansUI)
        {
            Destroy(item.Value.gameObject);
        }

        AllHumansUI.Clear();
    }

}