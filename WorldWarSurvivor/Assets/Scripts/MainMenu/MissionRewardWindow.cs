using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class MissionRewardWindow : Window
{
    private float LevelRequirement = 100;

    [SerializeField] private RectTransform unitsLayout;
    [SerializeField] private RectTransform ItemsLayout;

    [SerializeField] private TMP_Text MoneyText;
    [SerializeField] private Button ConfirmButton;

    [SerializeField] private UnitRewardItem UnitsPrefabs;
    [SerializeField] private Image ItemImagePrefabs;

    private List<UnitRewardItem> _createdUnits = new();
    private List<Image> _itemsForReward = new();

    private void Start()
    {
        StartCoroutine(Utilities.WaitAndRun(SetupWindow, 0.2f));
    }

    private void SetupWindow()
    {
        ConfirmButton.onClick.AddListener(Hide);
        ConfirmButton.onClick.AddListener(GetRewards);
        ConfirmButton.onClick.AddListener(ClearLists);


        if (BaseProgression.Instance.PlayerData.IsMissionCompleted)
        {
            BaseProgression.Instance.PlayerData.IsMissionCompleted = false;
            MoneyText.text = BaseProgression.Instance.PlayerData.CurrentMission.MoneyReward.ToString();

            RewardUnits();
            RewardItems();

            Show();
        }
    }

    private void RewardUnits()
    {
        var command = BaseProgression.Instance.PlayerData.CurrentCommand;

        foreach (var item in command)
        {
            var unitReward = Instantiate(UnitsPrefabs, unitsLayout);

            unitReward.experienceText.text = item.CurrentAmountOfExperience.ToString() + "/" + LevelRequirement;
            unitReward.ExperienceBar.value = item.CurrentAmountOfExperience / LevelRequirement;
            _createdUnits.Add(unitReward);
        }
    }

    private void RewardItems()
    {
        foreach (var item in BaseProgression.Instance.PlayerData.CurrentMission.ItemReward)
        {
            var im = Instantiate(ItemImagePrefabs, ItemsLayout);
            im.sprite = item.ItemImage;
            _itemsForReward.Add(im);
        }
    }

    private void GetRewards()
    {
        BaseProgression.Instance.PlayerData.Money += BaseProgression.Instance.PlayerData.CurrentMission.MoneyReward;

        foreach (var item in BaseProgression.Instance.PlayerData.CurrentMission.ItemReward)
            Utilities.AutoPlaceItem(BaseProgression.Instance.PlayerData.PlayerInventory, new InventoryItemInfo(item));

        BaseProgression.Instance.PlayerData.IsMissionCompleted = false;
    }

    private void ClearLists()
    {
        foreach (var item in _createdUnits)
            Destroy(item.gameObject);

        foreach (var item in _itemsForReward)
            Destroy(item.gameObject);

        _createdUnits.Clear();
        _itemsForReward.Clear();
    }
}
