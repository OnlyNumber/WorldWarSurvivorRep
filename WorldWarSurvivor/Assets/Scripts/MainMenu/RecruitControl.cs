using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RecruitControl : MonoBehaviour
{
    public int CountOfRecruits;

    [SerializeField]
    private List<GameObject> modelsForCreation;

    public Dictionary<HumanStats, HumanUIButton> CreatedRecruits = new();

    public RectTransform ParentForRecruitList;

    public HumanUIButton HumanUIPrefab;

    public GameObject RecruitWindow;

    public RectTransform PurchaseButton;

    public Button CloseButton;

    public Camera RecruitCamera;

    private void Start()
    {
        CloseButton.onClick.AddListener(CloseRecruitingWindow);
    }

    public void OpenRecruits()
    {
        if (CreatedRecruits.Count == 0)
            GenerateNewRecruits();

        RecruitWindow.SetActive(true);
    }

    public void GenerateNewRecruits()
    {
        for (int i = 0; i < CountOfRecruits; i++)
        {
            var generatedHuman = GenerateHuman();

            var humanUI = Instantiate(HumanUIPrefab, ParentForRecruitList);

            humanUI.CurrentLevel.text = generatedHuman.CurrentLevel.ToString();
            humanUI.HumanHealth.text = generatedHuman.HumanHealth.ToString();

            humanUI.MeleeSkill.text = generatedHuman.MeleeSkill.ToString();
            humanUI.RangeSkill.text = generatedHuman.RangeSkill.ToString();

            humanUI.GrabbingItem.OnDrop += () => TryPurchaseHuman(generatedHuman, humanUI, GetPosition());

            CreatedRecruits.Add(generatedHuman, humanUI);

        }
    }

    private Vector2 GetPosition()
    {
        return Input.mousePosition;
    }

    private void TryPurchaseHuman(HumanStats stats, HumanUIButton humanUI, Vector2 position)
    {
        if (UtilitiesUI.IsInsideOfSquareImage(PurchaseButton.position, PurchaseButton.rect, position))
        {
            BaseProgression.Instance.Rostert.Add(stats);
            Destroy(humanUI.gameObject);
        }
        else
        {
            humanUI.transform.SetParent(ParentForRecruitList);

            LayoutRebuilder.ForceRebuildLayoutImmediate(ParentForRecruitList);
        }
    }

    //Improved in future
    private HumanStats GenerateHuman()
    {
        HumanStats humanStats = new()
        {
            CurrentLevel = 0,
            CurrentAmountOfExperience = 0,

            HumanHealth = Random.Range(70, 100),

            MeleeSkill = Random.Range(30, 60),
            RangeSkill = Random.Range(30, 60),

            ModelPrefab = modelsForCreation[Random.Range(0, modelsForCreation.Count)]
        };

        return humanStats;
    }

    private void CloseRecruitingWindow()
    {
        RecruitWindow.SetActive(false);
        CameraControl.ChangeToCamera(CameraControl.MainCamera);
    }

    public void Dispose()
    {

    }
}
