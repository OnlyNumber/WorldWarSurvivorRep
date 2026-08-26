using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseMap : MonoBehaviour
{
    public const string ResourcesItemsPath = "ScriptableObjects/Items";

    //private int CurrentMap;
    #region  UI

    public Button CloseButton;
    public Button StartExpedition;
    public GameObject ChooseMapWindow;
    public MissionDataWindow MissionWindow;
    public Camera ChooseMapCamera;

    [SerializeField] private RectTransform ItemsLayout;
    [SerializeField] private Image ItemImagePrefabs;
    private List<Image> _itemsForReward = new();

    #endregion

    #region Private lists

    public List<Transform> MapParents;
    private Dictionary<Transform, List<Button>> missionButtons = new();
    private Dictionary<Button, MissionData> missionDatas = new();


    #endregion

    #region Variables for mission

    [SerializeField] private Button missionButtonPrefab;
    [SerializeField] private Vector2Int MissionsCountRange;
    [SerializeField] private int MoneyRewardForMission;

    #endregion


    private void Start()
    {
        CloseButton.onClick.AddListener(CloseWindow);
        CreateMissionButtons();

    }

    public void OpenExpedition()
    {
        ChooseMapWindow.SetActive(true);
    }

    private void ShowMissionRewards(MissionData mission)
    {
        BaseProgression.Instance.PlayerData.CurrentMission = mission;
        
        foreach (var item in _itemsForReward)
            Destroy(item.gameObject);

        _itemsForReward.Clear();

        foreach (var item in BaseProgression.Instance.PlayerData.CurrentMission.ItemReward)
        {
            if (item == null)
                Debug.Log("item == null");

            var im = Instantiate(ItemImagePrefabs, ItemsLayout);
            im.sprite = item.ItemImage;
            _itemsForReward.Add(im);
        }

        MissionWindow.SetMission(mission);
        MissionWindow.ShowWindow();


    }

    private void CreateMissionButtons()
    {
        for (int mapIndex = 0; mapIndex < MapParents.Count; mapIndex++)
        {
            int missionCount = UnityEngine.Random.Range(MissionsCountRange.x, MissionsCountRange.y);

            List<Button> currentButtonList = new();
            missionButtons.Add(MapParents[mapIndex], currentButtonList);

            for (int i = 0; i < missionCount; i++)
            {
                var currentButton = Instantiate(missionButtonPrefab, MapParents[mapIndex]);
                var missionData = GenerateMission();
                missionData.Map = (MissionMap)mapIndex;
                currentButton.GetComponentInChildren<TMP_Text>().text = missionData.Type.ToString();

                currentButton.onClick.AddListener(() => ShowMissionRewards(missionData));

                currentButtonList.Add(currentButton);
                missionDatas.Add(currentButton, missionData);
            }
        }
    }

    private void CloseWindow()
    {
        ChooseMapWindow.SetActive(false);
        MissionWindow.HideWindow();
    }

    private MissionData GenerateMission()
    {
        MissionData missionData = new();

        missionData.Type = (MissionType)UnityEngine.Random.Range(0, (int)MissionType.Count);
        missionData.LongOfMission = (MissionLong)UnityEngine.Random.Range(0, (int)MissionLong.Count);
        missionData.MoneyReward = MoneyRewardForMission * (int)missionData.LongOfMission;
        missionData.ItemReward.Add(FindItemForReward());
        missionData.ItemReward.Add(FindItemForReward());

        return missionData;
    }

    private InventoryItemSO FindItemForReward()
    {
        var inventoryItems = Resources.LoadAll<InventoryItemSO>(ResourcesItemsPath);

        return inventoryItems[UnityEngine.Random.Range(0, inventoryItems.Length)];
    }
}

[Serializable]
public class MissionData
{
    public MissionMap Map;
    public MissionType Type;
    public MissionLong LongOfMission;
    public int MoneyReward;
    public List<InventoryItemSO> ItemReward = new();
}

public enum MissionType
{
    KillToughEnemy,
    FindSomeObjects,
    DestroyObject,
    Count
}

public enum MissionLong
{
    Short = 1,
    Middle = 2,
    Long = 3,
    Count
}

public enum MissionMap
{
    Soviet,
    German,
    American,
    Count
}
