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
        for (int i = 0; i < BaseProgression.Instance.PlayerData.Roster.Count; i++)
        {
            HumanStats generatedHuman = BaseProgression.Instance.PlayerData.Roster[i];
            AllHumansUI.Add(generatedHuman, CreatePrefab(generatedHuman, RosterLists));
        }

        for (int i = 0; i < BaseProgression.Instance.PlayerData.CurrentCommand.Count; i++)
        {
            HumanStats generatedHuman = BaseProgression.Instance.PlayerData.CurrentCommand[i];
            AllHumansUI.Add(generatedHuman, CreatePrefab(generatedHuman, CommandLists));
        }

        HumanUIButton CreatePrefab(HumanStats generatedHuman, Transform parent)
        {
            HumanUIButton humanUI = Instantiate(HumanUIPrefab, parent);

            humanUI.HumanImageButton.onClick.AddListener(() => OpenInventory(generatedHuman.HumanInventoryInfo));

            humanUI.CurrentLevel.text = generatedHuman.CurrentLevel.ToString();
            humanUI.HumanHealth.text = generatedHuman.HumanHealth.ToString();

            humanUI.MeleeSkill.text = generatedHuman.MeleeSkill.ToString();
            humanUI.RangeSkill.text = generatedHuman.RangeSkill.ToString();

            humanUI.GrabbingItem.OnPickUp += () => TakeItem(generatedHuman);

            humanUI.GrabbingItem.OnDrop += () => TryAddHumanToCommand(generatedHuman, humanUI, GetPosition());

            return humanUI;
        }

    }

    #region Moving Human Button
    private Vector2 GetPosition()
    {
        return Input.mousePosition;
    }

    private void TakeItem(HumanStats stats)
    {
        var humanUI = AllHumansUI[stats];

        humanUI.transform.SetParent(TransferParent);

        if (BaseProgression.Instance.PlayerData.Roster.Contains(stats))
        {
            _lastProgressionList = BaseProgression.Instance.PlayerData.Roster;
            BaseProgression.Instance.PlayerData.Roster.Remove(stats);
        }
        else
        {
            _lastProgressionList = BaseProgression.Instance.PlayerData.CurrentCommand;
            BaseProgression.Instance.PlayerData.CurrentCommand.Remove(stats);
        }
    }

    private void TryAddHumanToCommand(HumanStats stats, HumanUIButton humanUI, Vector2 position)
    {
        if (UtilitiesUI.IsInsideOfSquareImage(CommandLists.position, CommandLists.rect, position))
            SetHumanUI(BaseProgression.Instance.PlayerData.CurrentCommand, CommandLists);
        else if (UtilitiesUI.IsInsideOfSquareImage(RosterLists.position, RosterLists.rect, position))
            SetHumanUI(BaseProgression.Instance.PlayerData.Roster, RosterLists);
        else
            SetHumanUI(_lastProgressionList, _lastTransformList);




        void SetHumanUI(List<HumanStats> list, RectTransform parent)
        {
            list.Add(stats);
            humanUI.transform.SetParent(parent);
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
        }
    }
    #endregion
    
    private void ClearWindow()
    {
        foreach (var item in AllHumansUI)
        {
            Destroy(item.Value.gameObject);
        }

        AllHumansUI.Clear();
    }

    #region Inventory Activation

    private void OpenInventory(HumanInventoryInfo HumanInventory)
    {
        InventoryWindow.Instance.OpenWindow(HumanInventory, BaseProgression.Instance.PlayerData.PlayerInventory);
    }

    #endregion
}