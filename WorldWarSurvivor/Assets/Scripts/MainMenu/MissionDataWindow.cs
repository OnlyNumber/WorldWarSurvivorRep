using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class MissionDataWindow : MonoBehaviour
{

    public TMP_Text MapText; 
    public TMP_Text TypeText; 
    public TMP_Text LongOfMissionText; 
    public TMP_Text MoneyRewardText; 

    public Button StartMissionButton;

    private void Start() {
        
        StartMissionButton.onClick.AddListener(StartExpedition);
    }
    
    public void SetMission(MissionData mission)
    {
        MapText.text = mission.Map.ToString();
        TypeText.text = mission.Type.ToString();
        LongOfMissionText.text = mission.LongOfMission.ToString();
        MoneyRewardText.text = mission.MoneyReward.ToString();
    }
    
    private void StartExpedition()
    {
        BaseProgression.Instance.SaveInfo();
        SceneManager.LoadScene(Utilities.GameplayTestSceneIndex);
    }

    #region ShowHide
    public void ShowWindow()
    {
        gameObject.SetActive(true);
    }

    public void HideWindow()
    {
        gameObject.SetActive(false);
    }
    #endregion

}
