using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitControl : MonoBehaviour
{
    public int CountOfRecruits;

    [SerializeField]
    private List<GameObject> modelsForCreation;

    public Dictionary<HumanStats, HumanUIButton> CreatedRecruits = new();

    public Transform ParentForRecruitList;

    public HumanUIButton HumanUIPrefab;

    public GameObject RecruitWindow;

    public Button CloseButton;

    public void OpenRecruits()
    {
        if (CreatedRecruits.Count == 0)
            GenerateNewRecruits();

        RecruitWindow.SetActive(true);
    }

    public void CloseRecruits()
    {
        RecruitWindow.SetActive(false);
        
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


            CreatedRecruits.Add(generatedHuman, humanUI);

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

    public void Dispose()
    {

    }
}
